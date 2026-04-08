using System.Net;
using System.Text.Json;

namespace Praetoria_demo.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ErrorHandlingMiddleware(
            RequestDelegate next,
            ILogger<ErrorHandlingMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault()
                    ?? context.TraceIdentifier;//fallback to trace id so we always have at least one identifier

                _logger.LogError(ex,
                    "Unhandled exception for {Method} {Path}. CorrelationId: {CorrelationId}",
                    context.Request.Method,
                    context.Request.Path,
                    correlationId);

                await HandleExceptionAsync(context, ex, correlationId, _env);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex, string correlationId, IHostEnvironment env)
        {
            var statusCode = ex switch
            {
                ArgumentException => HttpStatusCode.BadRequest,
                KeyNotFoundException => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError
            };

            var message = statusCode == HttpStatusCode.InternalServerError && env.IsProduction()
                ? "An unexpected error occurred"
                : ex.Message;

            var response = new
            {
                error = message,
                status = (int)statusCode,
                traceId = context.TraceIdentifier,
                correlationId
            };

            var json = JsonSerializer.Serialize(response);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(json);
        }
    }
}
