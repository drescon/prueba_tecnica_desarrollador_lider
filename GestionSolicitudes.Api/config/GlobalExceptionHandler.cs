using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GestionSolicitudes.Application.Exceptions;

namespace GestionSolicitudes.Api.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> _logger)
    {
        this._logger = _logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Ocurrió una excepción no controlada: {Message}", exception.Message);

        // Mapeo estructurado usando el estándar RFC 7807 (ProblemDetails)
        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path
        };

        switch (exception)
        {
            case InvalidCredentialsException:
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                problemDetails.Status = StatusCodes.Status401Unauthorized;
                problemDetails.Title = "No Autorizado";
                problemDetails.Detail = exception.Message;
                break;

            case UserNotActiveException:
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                problemDetails.Status = StatusCodes.Status403Forbidden;
                problemDetails.Title = "Acceso Prohibido";
                problemDetails.Detail = exception.Message;
                break;

            case EntityNotFoundException:
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                problemDetails.Status = StatusCodes.Status404NotFound;
                problemDetails.Title = "Entidad No Encontrada";
                problemDetails.Detail = exception.Message;
                break;

            case DuplicateRequestException:
                httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                problemDetails.Status = StatusCodes.Status409Conflict;
                problemDetails.Title = "Solicitud Duplicada";
                problemDetails.Detail = exception.Message;
                break;

            case InvalidStateTransitionException:
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Transición de Estado Inválida";
                problemDetails.Detail = exception.Message;
                break;

            case MissingObservationException:
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Observación Requerida";
                problemDetails.Detail = exception.Message;
                break;

            default:
                // Cualquier otra excepción del sistema (ej. caídas de BD) se vuelve un 500
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Title = "Error Interno del Servidor";
                problemDetails.Detail = "Ocurrió un error inesperado en el servidor. Por favor intente más tarde.";
                break;
        }

  
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}