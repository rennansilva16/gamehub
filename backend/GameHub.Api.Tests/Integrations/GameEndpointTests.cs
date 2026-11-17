
using System.Collections.Concurrent;
using System.Net;
using System.Net.Http.Json;
using GameHub.Api.Contracts.Games;
using GameHub.Api.Integrations.Rawg;
using GameHub.Api.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Moq;

namespace GameHub.Api.Tests.Integration;

public class GameEndpointTests
{
    private const string TestApiKey = "fake-rawg-api-key";

    [Fact]
    public async Task GetGames_WhenCatalogIsAvailable_ShouldReturnOkWithGames()
    {
        // Arrange
        var expectedGames = new List<GameSummaryDto>
        {
            new()
            {
                Id = 3328,
                Name = "The Witcher 3",
                BackgroundImage = "witcher-image.jpg"
            },
            new()
            {
                Id = 3498,
                Name = "Grand Theft Auto V",
                BackgroundImage = "gta-image.jpg"
            }
        };

        var serviceMock = new Mock<IGameCatalogService>();

        serviceMock
            .Setup(service => service.GetGamesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((IReadOnlyList<GameSummaryDto>)expectedGames);

        using var factory = new GameApiFactory(serviceMock.Object);
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/games");

        var games = await response.Content
            .ReadFromJsonAsync<List<GameSummaryDto>>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(games);
        Assert.Equal(2, games.Count);
        Assert.Equal("The Witcher 3", games[0].Name);
        Assert.Equal("Grand Theft Auto V", games[1].Name);
    }

    [Fact]
    public async Task GetGames_WhenRawgIntegrationFails_ShouldReturnBadGateway()
    {
        // Arrange
        var serviceMock = new Mock<IGameCatalogService>();

        serviceMock
            .Setup(service => service.GetGamesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new RawgApiException("Falha simulada da RAWG."));

        var loggerProvider = new TestLoggerProvider();

        using var factory = new GameApiFactory(
            serviceMock.Object,
            loggerProvider);

        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/games");
        var responseBody = await response.Content.ReadAsStringAsync();

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        Assert.Equal(HttpStatusCode.BadGateway, response.StatusCode);
        Assert.NotNull(problem);
        Assert.Equal(502, problem.Status);
        Assert.Equal("Falha na integração externa", problem.Title);
        Assert.Equal(
            "Não foi possível obter o catálogo de jogos no momento.",
            problem.Detail
        );

        // A chave fictícia não deve aparecer na resposta nem nos logs.
        Assert.DoesNotContain(TestApiKey, responseBody);
        Assert.DoesNotContain(
            TestApiKey,
            string.Join(Environment.NewLine, loggerProvider.Messages)
        );
    }

    [Fact]
    public async Task GetGames_WhenUnexpectedExceptionOccurs_ShouldReturnInternalServerError()
    {
        // Arrange
        var serviceMock = new Mock<IGameCatalogService>();

        serviceMock
            .Setup(service => service.GetGamesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("INTERNAL_ERROR_DETAILS"));

        using var factory = new GameApiFactory(serviceMock.Object);
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/games");
        var responseBody = await response.Content.ReadAsStringAsync();

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.NotNull(problem);
        Assert.Equal(500, problem.Status);
        Assert.Equal("Erro interno do servidor", problem.Title);

        Assert.DoesNotContain("INTERNAL_ERROR_DETAILS", responseBody);
        Assert.Equal(
            "Ocorreu um erro inesperado ao processar a requisição.",
            problem.Detail
        );
    }
}

// Factory responsável por inicializar a API em ambiente de teste.
public class GameApiFactory : WebApplicationFactory<Program>
{
    private readonly IGameCatalogService _catalogService;
    private readonly TestLoggerProvider _loggerProvider;

    public GameApiFactory(
        IGameCatalogService catalogService,
        TestLoggerProvider? loggerProvider = null)
    {
        _catalogService = catalogService;
        _loggerProvider = loggerProvider ?? new TestLoggerProvider();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, configuration) =>
        {
            configuration.AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    ["Rawg:BaseUrl"] = "https://example.com/api/",
                    ["Rawg:ApiKey"] = "fake-rawg-api-key"
                }
            );
        });

        builder.ConfigureLogging(logging =>
        {
            logging.AddProvider(_loggerProvider);
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IGameCatalogService>();
            services.AddSingleton(_catalogService);
        });
    }
}

// Provedor simples para capturar os logs durante os testes.
public class TestLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentQueue<string> _messages = new();

    public IEnumerable<string> Messages => _messages;

    public ILogger CreateLogger(string categoryName)
    {
        return new TestLogger(_messages);
    }

    public void Dispose()
    {
    }

    private class TestLogger : ILogger
    {
        private readonly ConcurrentQueue<string> _messages;

        public TestLogger(ConcurrentQueue<string> messages)
        {
            _messages = messages;
        }

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            var message = formatter(state, exception);

            if (exception is not null)
            {
                message += Environment.NewLine + exception;
            }

            _messages.Enqueue(message);
        }
    }
}
