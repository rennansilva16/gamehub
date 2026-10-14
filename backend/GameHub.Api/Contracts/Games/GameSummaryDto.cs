namespace GameHub.Api.Contracts.Games;

  public class GameSummaryDto
  {
      public int Id { get; set; }
      public string Name { get; set; } = string.Empty;
      public string BackgroundImage { get; set; } = string.Empty;
  }
