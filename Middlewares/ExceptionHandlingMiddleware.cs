using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Luna.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Передаём запрос дальше по конвейеру
                await _next(context);
            }
            catch (Exception ex)
            {
                // Логируем полную информацию об ошибке
                _logger.LogError(ex, "Необработанное исключение при обработке запроса {Path}", context.Request.Path);

                // Определяем HTTP-статус в зависимости от типа исключения
                var statusCode = ex switch
                {
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,
                    UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                    ArgumentException => (int)HttpStatusCode.BadRequest,
                    _ => (int)HttpStatusCode.InternalServerError
                };

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                // Формируем ProblemDetails – стандартный объект ошибки
                var problemDetails = new ProblemDetails
                {
                    Status = statusCode,
                    Title = GetTitleForStatus(statusCode),
                    Detail = _environment.IsDevelopment() ? ex.Message : "Произошла внутренняя ошибка сервера."
                };

                // В режиме разработки можно добавить стектрейс (опционально)
                if (_environment.IsDevelopment())
                {
                    problemDetails.Extensions["traceId"] = context.TraceIdentifier;
                }

                var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(problemDetails, jsonOptions);
                await context.Response.WriteAsync(json);
            }
        }

        private static string GetTitleForStatus(int statusCode) => statusCode switch
        {
            400 => "Bad Request",
            401 => "Unauthorized",
            404 => "Not Found",
            500 => "Internal Server Error",
            _ => "Error"
        };
    }
}
