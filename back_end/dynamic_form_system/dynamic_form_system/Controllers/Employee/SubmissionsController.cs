using dynamic_form_system.DTOs.Requests;
using dynamic_form_system.DTOs.Responses;
using dynamic_form_system.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace dynamic_form_system.Controllers.Employee
{
    [ApiController]
    public class SubmissionsController : ControllerBase
    {
        private readonly IFormService _formService;
        private readonly ISubmissionService _submissionService;
        public SubmissionsController(IFormService formService, ISubmissionService submissionService)
        {
            _formService = formService;
            _submissionService = submissionService;
        }

        // 1. Danh sách form active, sắp theo thứ tự
        // Endpoint: GET /api/forms/active
        [HttpGet("api/forms/active")]
        public async Task<ActionResult<ApiResponse<IEnumerable<FormAdminListDto>>>> GetActiveForms()
        {
            // Gọi hàm lấy danh sách form có Status = "Active" từ Service
            var activeForms = await _formService.GetActiveFormsAsync();

            return Ok(new ApiResponse<IEnumerable<FormAdminListDto>>
            {
                Message = "Lấy danh sách Form thành công",
                Data = activeForms
            });
        }

        /// <summary>
        /// 2. Nhân viên submit form
        /// Endpoint: POST /api/forms/{id}/submit
        /// </summary>
        [HttpPost("api/forms/{id:guid}/submit")]
        public async Task<ActionResult<ApiResponse<object>>> SubmitForm([FromRoute] Guid id, [FromBody] SubmitFormRequestDto request)
        {
            // TODO: Sau này làm tính năng Đăng nhập, bạn sẽ lấy UserID từ Token ở đây
            Guid? currentUserId = null;

            // Logic kiểm tra JSON, validate dữ liệu đã có Validator gánh vác
            await _submissionService.SubmitAsync(id, currentUserId, request);

            return Ok(new ApiResponse<object>
            {
                Message = "Nộp form thành công!"
            });
        }

        /// <summary>
        /// 3. Xem lại danh sách bài đã submit
        /// Endpoint: GET /api/submissions
        /// </summary>
        [HttpGet("api/submissions")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SubmissionHistoryDto>>>> GetMySubmissions()
        {
            // TODO: Lấy UserID thực tế từ JWT Token
            Guid? currentUserId = null;

            var history = await _submissionService.GetMySubmissionsAsync(currentUserId);

            return Ok(new ApiResponse<IEnumerable<SubmissionHistoryDto>>
            {
                Message = "Lấy lịch sử nộp bài thành công",
                Data = history
            });
        }
    }
}
