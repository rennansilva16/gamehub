using GameHub.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameHub.Api.Controllers;

  [ApiController]
  [Route("api/games")]
  public class GameController : ControllerBase
  {
    private readonly IGameCatalogService _gameCatalogService;

    public GameController(IGameCatalogService gameCatalogService)
    {
      _gameCatalogService = gameCatalogService;
    }

    // Adicionar rota para obter a lista de jogos do catálogo
    [HttpGet]
    public async Task<IActionResult> GetGames(CancellationToken cancellationToken)
    {
      var games = await _gameCatalogService.GetGamesAsync(cancellationToken);
      return Ok(games);
    }
  }
