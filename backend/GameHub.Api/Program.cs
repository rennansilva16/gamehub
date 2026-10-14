using GameHub.Api.Integrations.Rawg;
using GameHub.Api.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services
    .AddOptions<RawgOptions>()
    .Bind(builder.Configuration.GetSection(RawgOptions.SectionName))
    .Validate(
        options => Uri.TryCreate(
            options.BaseUrl,
            UriKind.Absolute,
            out var uri)
            && uri.Scheme == Uri.UriSchemeHttps,
        "A URL-base da RAWG deve ser uma URL HTTPS válida.")
    .Validate(
        options => !string.IsNullOrWhiteSpace(options.ApiKey),
        "A chave da API da RAWG não foi configurada.")
    .ValidateOnStart();

builder.Services.AddHttpClient<IRawgClient, RawgClient>(
    (serviceProvider, httpClient) =>
    {
      var options = serviceProvider
          .GetRequiredService<IOptions<RawgOptions>>()
          .Value;

      httpClient.BaseAddress = new Uri(options.BaseUrl);
      httpClient.Timeout = TimeSpan.FromSeconds(15);
    });

builder.Services.AddScoped<IGameCatalogService, CatalogService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
  var forecast = Enumerable.Range(1, 5).Select(index =>
      new WeatherForecast
      (
          DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
          Random.Shared.Next(-20, 55),
          summaries[Random.Shared.Next(summaries.Length)]
      ))
      .ToArray();
  return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
  public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
