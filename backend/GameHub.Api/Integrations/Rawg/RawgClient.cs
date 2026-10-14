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

    try
    {
      var apiKey = Uri.EscapeDataString(_rawgOptions.ApiKey);
      var endpoint = $"games?key={apiKey}&page_size=20";
      Console.WriteLine($"RAWG API Endpoint: {endpoint}");
      Console.WriteLine($"RAWG API Key: {apiKey}");

      using var response = await _httpClient.GetAsync(
        endpoint,
        cancellationToken
      );

      if (!response.IsSuccessStatusCode)
      {
        throw new RawgApiException($"A RAWG retornou o status HTTP {(int)response.StatusCode}.");
      }

      var result = await response.Content.ReadFromJsonAsync<RawgGamesResponseDto>(cancellationToken: cancellationToken);

      if (result is null)
      {
        throw new RawgApiException("A API da RAWG retornou uma resposta vazia ou inválida.");
      }

      return result.results;
    }
    catch (OperationCanceledException)
    when (cancellationToken.IsCancellationRequested)
    {
      throw;
    }
    catch (OperationCanceledException ex)
    {
      throw new RawgApiException("A requisição à RAWG excedeu o tempo limite.", ex);
    }
    catch (HttpRequestException ex)
    {
      throw new RawgApiException("Não foi possível estabelecer comunicação com a RAWG.", ex);
    }
    catch (JsonException ex)
    {
      throw new RawgApiException("Não foi possível interpretar a resposta da RAWG.", ex);
    }
  }
}
