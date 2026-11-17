using GameHub.Api.Integrations.Rawg;
using GameHub.Api.Services;
using Moq;

namespace GameHub.Api.Tests.Services;

public class CatalogServiceTests
{
  [Fact]
  public async Task GetGamesAsync_WhenRawgClientThrowsException_ShouldPropagateException()
  {
    // Arrange
    var rawgClientMock = new Mock<IRawgClient>();

    rawgClientMock
        .Setup(client => client.GetGamesAsync(It.IsAny<CancellationToken>()))
        .ThrowsAsync(new RawgApiException("Falha simulada da RAWG."));

    var catalogService = new CatalogService(rawgClientMock.Object);

    // Act & Assert
    var exception = await Assert.ThrowsAsync<RawgApiException>(
        () => catalogService.GetGamesAsync()
    );

    Assert.Equal("Falha simulada da RAWG.", exception.Message);

    rawgClientMock.Verify(
        client => client.GetGamesAsync(It.IsAny<CancellationToken>()),
        Times.Once
    );
  }

  [Fact]
  public async Task GetGamesAsync_WhenRawgReturnsNoGames_ShouldReturnEmptyList()
  {
    // Arrange
    var rawgClientMock = new Mock<IRawgClient>();

    rawgClientMock
        .Setup(client => client.GetGamesAsync(It.IsAny<CancellationToken>()))
        .ReturnsAsync(new List<RawgGameDto>());

    var catalogService = new CatalogService(rawgClientMock.Object);

    // Act
    var games = await catalogService.GetGamesAsync();

    // Assert
    Assert.NotNull(games);
    Assert.Empty(games);

    rawgClientMock.Verify(
        client => client.GetGamesAsync(It.IsAny<CancellationToken>()),
        Times.Once
    );
  }

  [Fact]
  public async Task GetGamesAsync_WhenRawgReturnsGames_ShouldMapToGameSummaryDto()
  {
    // Arrange

    // Criamos um mock da interface IRawgClient.
    var rawgClientMock = new Mock<IRawgClient>();

    // Criamos jogos fictícios que simulam a resposta da RAWG.
    var rawgGames = new List<RawgGameDto>
        {
            new()
            {
                id = 3328,
                name = "The Witcher 3",
                slug = "the-witcher-3",
                released = "2015-05-18",
                background_image = "witcher-image.jpg"
            },
            new()
            {
                id = 3498,
                name = "Grand Theft Auto V",
                slug = "grand-theft-auto-v",
                released = "2013-09-17",
                background_image = "gta-image.jpg"
            }
        };

    // Configuramos o mock:
    // Quando GetGamesAsync for chamado, ele devolverá rawgGames.
    rawgClientMock
        .Setup(client => client.GetGamesAsync(It.IsAny<CancellationToken>()))
        .ReturnsAsync(rawgGames);

    // Criamos o serviço utilizando o mock.
    var catalogService = new CatalogService(rawgClientMock.Object);

    // Act

    // Executamos o método que queremos testar.
    var games = await catalogService.GetGamesAsync();

    // Assert

    // Verificamos se foram retornados dois jogos.
    Assert.Equal(2, games.Count);

    // Verificamos o mapeamento do primeiro jogo.
    Assert.Equal(3328, games[0].Id);
    Assert.Equal("The Witcher 3", games[0].Name);
    Assert.Equal("witcher-image.jpg", games[0].BackgroundImage);

    // Verificamos o mapeamento do segundo jogo.
    Assert.Equal(3498, games[1].Id);
    Assert.Equal("Grand Theft Auto V", games[1].Name);
    Assert.Equal("gta-image.jpg", games[1].BackgroundImage);

    // Verificamos se o serviço chamou o cliente RAWG.
    rawgClientMock.Verify(
        client => client.GetGamesAsync(It.IsAny<CancellationToken>()),
        Times.Once
    );
  }
}
