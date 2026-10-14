using GameHub.Api.Integrations.Rawg;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GameHub.Api.Exceptions;

  public class GlobalExceptionHandler : IExceptionHandler
  {
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (httpContext.RequestAborted.IsCancellationRequested)
        {
            return false;
        }

        var isRawgError = exception is RawgApiException;

        var statusCode = isRawgError
            ? StatusCodes.Status502BadGateway
            : StatusCodes.Status500InternalServerError;

        _logger.LogError(
            exception,
            "Erro ao processar a requisição. Status HTTP: {StatusCode}",
            statusCode);

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = isRawgError
                ? "Falha na integração externa"
                : "Erro interno do servidor",
            Detail = isRawgError
                ? "Não foi possível obter o catálogo de jogos no momento."
                : "Ocorreu um erro inesperado ao processar a requisição."
        };

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(
            problem,
            cancellationToken);

        return true;
    }

  }
