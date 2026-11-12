
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

    var options = Options.Create(new RawgOptions{
      ApiKey = "test-api-key",
      BaseUrl = "https://api.rawg.io/api/"
    });

    httpClient.BaseAddress = new Uri(options.Value.BaseUrl);

    var client = new RawgClient(httpClient, options);

    // Act & Assert
    var exception = await Assert.ThrowsAsync<RawgApiException>(() => client.GetGamesAsync());
    Assert.Equal("A API da RAWG retornou uma resposta vazia ou inválida.", exception.Message);
  }

  // TODO: Validar o tratamento de falhas de comunicação HTTP.

  // TODO: Validar o tratamento de timeout.

  // TODO: Validar o comportamento quando o chamador cancela a requisição.

  // TODO: Validar a montagem da requisição e seus parâmetros, sem expor a chave da API.

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
    public HttpRequestMessage? Request {get; private set; }
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
}
