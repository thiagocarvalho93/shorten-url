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
        HttpStatusCode status;
        string message;

        Type? exceptionType = exception.GetType();

        if (exceptionType == typeof(DirectoryNotFoundException) ||
            exceptionType == typeof(DllNotFoundException) ||
            exceptionType == typeof(EntryPointNotFoundException) ||
            exceptionType == typeof(FileNotFoundException) ||
            exceptionType == typeof(NotFoundException) ||
            exceptionType == typeof(KeyNotFoundException))
        {
            message = exception.Message;
            status = HttpStatusCode.NotFound;
        }
        else if (exceptionType == typeof(NotImplementedException))
        {
            status = HttpStatusCode.NotImplemented;
            message = exception.Message;
        }
        else if (exceptionType == typeof(UnauthorizedAccessException) ||
            exceptionType == typeof(AuthenticationException))
        {
            status = HttpStatusCode.Unauthorized;
            message = exception.Message;
        }
        else
        {
            status = HttpStatusCode.InternalServerError;
            message = exception.Message;
        }

        string exceptionResult = System.Text.Json.JsonSerializer.Serialize(new { error = message, status });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;

        return context.Response.WriteAsync(exceptionResult);
    }
}