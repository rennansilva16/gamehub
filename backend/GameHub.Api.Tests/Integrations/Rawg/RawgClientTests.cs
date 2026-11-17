
using System.Net;
using System.Text;
using GameHub.Api.Integrations.Rawg;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace GameHub.Api.Tests.Integrations.Rawg;

public class RawgClientTests
{



  // Validar o tratamento de uma resposta JSON nula.
  [Fact]
  public async Task GetGamesAsync_WhenResponseJsonIsNull_ShouldThrowRawgApiException()
  {
    // Arrange
    var handler = new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK)
    {
      Content = new StringContent(
        "null",
        Encoding.UTF8,
        "application/json"
      )
    });

    var httpClient = new HttpClient(handler);

    var options = Options.Create(new RawgOptions
    {
      ApiKey = "test-api-key",
      BaseUrl = "https://api.rawg.io/api/"
    });

    httpClient.BaseAddress = new Uri(options.Value.BaseUrl);

    var client = new RawgClient(httpClient, options);

    // Act & Assert
    var exception = await Assert.ThrowsAsync<RawgApiException>(() => client.GetGamesAsync());
    Assert.Equal("A API da RAWG retornou uma resposta vazia ou inválida.", exception.Message);
  }

  // Validar o tratamento de falhas de comunicação HTTP.
  [Fact]
  public async Task GetGamesAsync_WhenHttpRequestFails_ShouldThrowRawgApiException()
  {
    // Arrange
    // Cria o handler que simula falha de comunicação HTTP.
    var handler = new HttpRequestExceptionHandler();

    var httpClient = new HttpClient(handler)
    {
      BaseAddress = new Uri("https://api.rawg.io/api/")
    };

    var options = Options.Create(new RawgOptions
    {
      ApiKey = "test-api-key",
      BaseUrl = "https://api.rawg.io/api/"
    });

    var client = new RawgClient(httpClient, options);

    // Act & Assert

    // Verifica se o método GetGamesAsync lança RawgApiException com a mensagem correta.
    var exception = await Assert.ThrowsAsync<RawgApiException>(() => client.GetGamesAsync());

    Assert.Equal("Não foi possível estabelecer comunicação com a RAWG.", exception.Message);

  }

  // Validar o tratamento de timeout.
  [Fact]
  public async Task GetGamesAsync_WhenRequestTimesOut_ShouldThrowRawgApiException()
  {
    // Arrange
    // Utilizamos o handler que aguarda indefinidamente, até o token de cancelamento ser acionado.
    var handler = new CancellationHttpMessageHandler();

    var httpClient = new HttpClient(handler)
    {
      BaseAddress = new Uri("https://api.rawg.io/api/"),

      //Configuramos o timeout do HttpClient para um valor baixo, para simular o timeout.
      Timeout = TimeSpan.FromMilliseconds(100)
    };

    var options = Options.Create(new RawgOptions
    {
      ApiKey = "test-api-key",
      BaseUrl = "https://api.rawg.io/api/"
    });

    var client = new RawgClient(httpClient, options);

    // Act
    // Inicia a requisição, que será interrompida pelo timeout do HttpClient.
    var requestTask = client.GetGamesAsync();

    // Aguarda até o handler confirmar que recebeu a requisição
    await handler.RequestStarted.Task;

    // Assert
    // Verifica se o método propagou RawgApiException com a mensagem correta.
    var exception = await Assert.ThrowsAsync<RawgApiException>(() => requestTask);

    // A mensagem de exceção deve indicar que a requisição excedeu o tempo limite.
    Assert.Equal("A requisição à RAWG excedeu o tempo limite.", exception.Message);
  }

  // Validar o comportamento quando o chamador cancela a requisição.

  [Fact]
  public async Task GetGamesAsync_WhenCallerCancelsRequest_ShouldThrowOperationCanceledException()
  {
    // Arrange

    // Cria uma fonte de cancelamento.
    // Ela será responsável por solicitar o cancelamento da operação.
    using var cts = new CancellationTokenSource();

    // Nosso handler específico ficará aguardando o cancelamento.
    var handler = new CancellationHttpMessageHandler();

    var httpClient = new HttpClient(handler);

    var options = Options.Create(new RawgOptions
    {
      ApiKey = "test-api-key",
      BaseUrl = "https://api.rawg.io/api/"
    });

    httpClient.BaseAddress = new Uri(options.Value.BaseUrl);

    var client = new RawgClient(httpClient, options);

    // Act

    // Inicia a requisição, passando o token de cancelamento.
    var requestTask = client.GetGamesAsync(cts.Token);

    // Aguarda até termos certeza de que o handler começou a executar.
    await handler.RequestStarted.Task;

    // Solicita o cancelamento enquanto a requisição está em andamento.
    cts.Cancel();

    // Assert

    // Verifica se o método propagou OperationCanceledException.
    // Não deve transformar o cancelamento em RawgApiException.
    await Assert.ThrowsAnyAsync<OperationCanceledException>(
        () => requestTask
    );
  }


  // Validar a montagem da requisição e seus parâmetros, sem expor a chave da API.
  [Fact]
  public async Task GetGamesAsync_ShouldSendRequestWithCorrectParameters()
  {
    // Arrange
    var handler = new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK)
    {
      Content = new StringContent(
        """
        {
          "results": []
        }
        """,
        Encoding.UTF8,
        "application/json"
      )
    });

    var httpClient = new HttpClient(handler);

    var options = Options.Create(new RawgOptions
    {
      ApiKey = "test-api-key",
      BaseUrl = "https://api.rawg.io/api/"
    });

    httpClient.BaseAddress = new Uri(options.Value.BaseUrl);

    var client = new RawgClient(httpClient, options);

    // Act
    await client.GetGamesAsync();

    // Assert
    Assert.NotNull(handler.Request);

    Assert.Equal(HttpMethod.Get, handler.Request.Method);
    Assert.Equal("/api/games", handler.Request.RequestUri?.AbsolutePath);

    var query = QueryHelpers.ParseQuery(handler.Request!.RequestUri!.Query);

    Assert.Equal(options.Value.ApiKey, query["key"]);

    Assert.True(int.TryParse(query["page_size"], out var pageSize));
    Assert.True(pageSize > 0);
  }

  // Validar o tratamento de JSON inválido.
  [Fact]
  public async Task GetGamesAsync_WhenResponseHasInvalidJson_ShouldThrowRawgApiException()
  {
    // Arrange
    var handler = new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK)
    {
      Content = new StringContent(
        "Invalid JSON",
        Encoding.UTF8,
        "application/json"
        )
    });

    var httpClient = new HttpClient(handler);

    var options = Options.Create(new RawgOptions
    {
      ApiKey = "test-api-key",
      BaseUrl = "https://api.rawg.io/api/"
    });
    httpClient.BaseAddress = new Uri(options.Value.BaseUrl);

    var client = new RawgClient(httpClient, options);

    // Act & Assert
    var exception = await Assert.ThrowsAsync<RawgApiException>(() => client.GetGamesAsync());
    Assert.Equal("Não foi possível interpretar a resposta da RAWG.", exception.Message);
  }

  [Fact]
  public async Task GetGamesAsync_WhenResponseIsSuccessful_ShouldReturnGames()
  {
    // Arrange
    var json = """
    {
      "results": [
          {}
      ]
    }
    """;

    var handler = new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK)
    {
      Content = new StringContent(
        json,
        Encoding.UTF8,
        "application/json"
        )
    });

    var httpClient = new HttpClient(handler);

    var options = Options.Create(new RawgOptions
    {
      ApiKey = "test-api-key",
      BaseUrl = "https://api.rawg.io/api/"
    });
    httpClient.BaseAddress = new Uri(options.Value.BaseUrl);

    var client = new RawgClient(httpClient, options);

    //Act
    var games = await client.GetGamesAsync();

    // Assert
    Assert.Single(games);
  }

  // Validar que o método GetGamesAsync lança uma exceção RawgApiException quando a resposta da API RAWG não é bem-sucedida (código de status HTTP diferente de 200 OK).
  [Theory]
  [InlineData(HttpStatusCode.BadRequest)]
  [InlineData(HttpStatusCode.Unauthorized)]
  [InlineData(HttpStatusCode.NotFound)]
  [InlineData(HttpStatusCode.InternalServerError)]
  public async Task GetGamesAsync_WhenResponseIsError_ShouldThrowRawgApiException(HttpStatusCode statusCode)
  {
    // Arrange
    var handler = new FakeHttpMessageHandler(new HttpResponseMessage(statusCode)
    {
      Content = new StringContent(
        "Error message",
        Encoding.UTF8,
        "application/json"
        )
    });

    var httpClient = new HttpClient(handler);

    var options = Options.Create(new RawgOptions
    {
      ApiKey = "test-api-key",
      BaseUrl = "https://api.rawg.io/api/"
    });
    httpClient.BaseAddress = new Uri(options.Value.BaseUrl);

    var client = new RawgClient(httpClient, options);

    // Act & Assert
    var exception = await Assert.ThrowsAsync<RawgApiException>(() => client.GetGamesAsync());
    Assert.Equal($"A RAWG retornou o status HTTP {(int)statusCode}.", exception.Message);
  }


  private class FakeHttpMessageHandler : HttpMessageHandler
  {
    private readonly HttpResponseMessage _response;
    public HttpRequestMessage? Request { get; private set; }
    public FakeHttpMessageHandler(HttpResponseMessage response)
    {
      _response = response;
    }
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
      Request = request;
      return Task.FromResult(_response);
    }
  }

  private class CancellationHttpMessageHandler : HttpMessageHandler
  {
    // Sinaliza ao teste que a requisição chegou ao handler.
    public TaskCompletionSource<bool> RequestStarted { get; } =
        new(TaskCreationOptions.RunContinuationsAsynchronously);

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
      // Informa ao teste que começamos a processar a requisição.
      RequestStarted.TrySetResult(true);

      // Aguarda indefinidamente, até o token ser cancelado.
      // O cancelamento provoca OperationCanceledException.
      await Task.Delay(Timeout.Infinite, cancellationToken);

      // Não será alcançado neste teste, pois o cancelamento interrompe a espera.
      throw new InvalidOperationException("A requisição deveria ter sido cancelada.");
    }
  }

  private class HttpRequestExceptionHandler : HttpMessageHandler
  {
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
      throw new HttpRequestException("Simulated HTTP request failure.");
    }
  }
}
