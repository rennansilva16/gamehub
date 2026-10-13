namespace GameHub.Api.Integrations.Rawg;

public interface IRawgClient
{
    Task<IReadOnlyList<RawgGameDto>> GetGamesAsync(CancellationToken cancellationToken = default);
}
