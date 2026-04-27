using dynamic_form_system.DTOs.Responses;
using System.Net;
using System.Text.Json;

namespace dynamic_form_system.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi hệ thống không xác định: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            var message = "Đã xảy ra lỗi hệ thống nghiêm trọng. Vui lòng liên hệ Admin.";
            if (exception is KeyNotFoundException)
            {
                statusCode = HttpStatusCode.NotFound; 
                message = exception.Message; 
            }
            else if (exception is ArgumentException || exception is InvalidOperationException)
            {
                statusCode = HttpStatusCode.BadRequest; 
                message = exception.Message; 
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var errorResponse = new ApiResponse<object>
            {
                Success = false,
                Message = message,
                Data = null,
                Errors = null 
            };
            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var result = JsonSerializer.Serialize(errorResponse, jsonOptions);
            return context.Response.WriteAsync(result);
        }
    }
}

