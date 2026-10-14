using GameHub.Api.Contracts.Games;

namespace GameHub.Api.Services;

public interface IGameCatalogService
{
  Task<IReadOnlyList<GameSummaryDto>> GetGamesAsync(CancellationToken cancellationToken = default);
}
