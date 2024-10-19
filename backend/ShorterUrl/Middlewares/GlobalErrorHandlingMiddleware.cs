using System.Net;
using System.Security.Authentication;
using ShorterUrl.Exceptions;

namespace ShorterUrl.Middlewares;

public class GlobalErrorHandlingMiddleware
{
    private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public GlobalErrorHandlingMiddleware(ILogger<GlobalErrorHandlingMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error: {message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (status, message) = exception switch
        {
            // Client's fault
            DirectoryNotFoundException or
            DllNotFoundException or
            EntryPointNotFoundException or
            FileNotFoundException or
            NotFoundException or
            KeyNotFoundException => (HttpStatusCode.NotFound, exception.Message),

            ValidationException => (HttpStatusCode.BadRequest, exception.Message),

            UnauthorizedAccessException or
            AuthenticationException => (HttpStatusCode.Unauthorized, exception.Message),

            // My fault
            NotImplementedException => (HttpStatusCode.NotImplemented, exception.Message),

            _ => (HttpStatusCode.InternalServerError, exception.Message)
        };

        var exceptionResult = System.Text.Json.JsonSerializer.Serialize(new { error = message, status });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;

        return context.Response.WriteAsync(exceptionResult);
    }
}