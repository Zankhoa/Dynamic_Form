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
            // 1. Mặc định là lỗi 500 (Lỗi Server)
            var statusCode = HttpStatusCode.InternalServerError;
            var message = "Đã xảy ra lỗi hệ thống nghiêm trọng. Vui lòng liên hệ Admin.";

            // 2. Tùy biến mã lỗi HTTP dựa trên loại Exception (Bắt bệnh)
            if (exception is KeyNotFoundException)
            {
                statusCode = HttpStatusCode.NotFound; // 404
                message = exception.Message; // Vd: "Không tìm thấy Form"
            }
            else if (exception is ArgumentException || exception is InvalidOperationException)
            {
                statusCode = HttpStatusCode.BadRequest; // 400
                message = exception.Message; // Vd: "Trùng tên Field"
            }
            // Nếu bạn dùng FluentValidation, có thể bắt thêm ValidationException ở đây
            // else if (exception is ValidationException valEx) { ... }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var errorResponse = new ApiResponse<object>
            {
                Success = false,
                Message = message,
                Data = null,
                Errors = null // Chỗ này có thể map chi tiết lỗi nếu là ValidationException
            };

            // Ép kiểu JSON viết thường chữ cái đầu (camelCase) cho đúng chuẩn Javascript
            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var result = JsonSerializer.Serialize(errorResponse, jsonOptions);

            return context.Response.WriteAsync(result);
        }
    }
}

