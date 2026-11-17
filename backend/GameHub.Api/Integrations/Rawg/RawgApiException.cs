namespace GameHub.Api.Integrations.Rawg;

public class RawgApiException : Exception
{
  public RawgApiException(string message) : base(message) { }
  public RawgApiException(string message, Exception innerException) : base(message, innerException) { }
}
