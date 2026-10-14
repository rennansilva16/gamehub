using GameHub.Api.Contracts.Games;
using GameHub.Api.Integrations.Rawg;

namespace GameHub.Api.Services;

  public class CatalogService : IGameCatalogService
  {
    private readonly IRawgClient _rawgClient;

  public CatalogService(IRawgClient rawgClient)
  {
    _rawgClient = rawgClient;
  }

  public async Task<IReadOnlyList<GameSummaryDto>> GetGamesAsync(CancellationToken cancellationToken = default)
    {
      // Implement the logic to retrieve games from the catalog
      var games = await _rawgClient.GetGamesAsync(cancellationToken);
      var gameSummaries = games.Select(g => new GameSummaryDto
      {
        Id = g.id,
        Name = g.name,
        BackgroundImage = g.background_image ?? string.Empty
      }).ToList();
      // For now, return an empty list as a placeholder
      return gameSummaries;
    }
  }
