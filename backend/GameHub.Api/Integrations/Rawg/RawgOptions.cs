namespace GameHub.Api.Integrations.Rawg;

  public class RawgOptions
  {
    public const string SectionName = "Rawg";
    public string BaseUrl { get; set; } = null!;
    public string ApiKey { get; set; } = null!;
  }
