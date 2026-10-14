using System.Text.Json;
using Microsoft.Extensions.Options;

namespace GameHub.Api.Integrations.Rawg;

public class RawgClient : IRawgClient
{
    private readonly HttpClient _httpClient;
    private readonly RawgOptions _rawgOptions;

    public RawgClient(HttpClient httpClient, IOptions<RawgOptions> options)
    {
        _httpClient = httpClient;
        _rawgOptions = options.Value;
    }

    public async Task<IReadOnlyList<RawgGameDto>> GetGamesAsync(CancellationToken cancellationToken = default)
    {

        var apiKey = Uri.EscapeDataString(_rawgOptions.ApiKey);
        var endpoint = $"games?key={apiKey}&page_size=20";
        Console.WriteLine($"RAWG API Endpoint: {endpoint}");
        Console.WriteLine($"RAWG API Key: {apiKey}");

        using var response = await _httpClient.GetAsync(
          endpoint,
          cancellationToken
        );

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<RawgGamesResponseDto>(cancellationToken: cancellationToken);

        if (result is null)
        {
            throw new InvalidOperationException("A API da RAWG retornou uma resposta vazia ou inválida.");
        }

        return result.results;
    }


}
