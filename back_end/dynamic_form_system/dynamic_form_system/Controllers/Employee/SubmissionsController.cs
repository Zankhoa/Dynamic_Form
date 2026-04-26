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

        //  GET /api/forms/active -- get list data form active
        [HttpGet("api/forms/active")]
        public async Task<ActionResult<ApiResponse<IEnumerable<FormAdminListDto>>>> GetActiveForms()
        {
            var activeForms = await _formService.GetActiveFormsAsync();
            return Ok(new ApiResponse<IEnumerable<FormAdminListDto>>
            {
                Message = "Lấy danh sách Form thành công",
                Data = activeForms
            });
        }

        // POST /api/forms/{id}/submit -- submit form data
        [HttpPost("api/forms/{id:guid}/submit")]
        public async Task<ActionResult<ApiResponse<object>>> SubmitForm([FromRoute] Guid id, [FromBody] SubmitFormRequestDto request)
        {
            await _submissionService.SubmitAsync(id, request);
            return Ok(new ApiResponse<object>
            {
                Message = "Nộp form thành công!"
            });
        }

        // GET /api/submissions -- get data submission submited
        [HttpGet("api/submissions")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SubmissionHistoryDto>>>> GetMySubmissionsAsync()
        {
            Guid? currentUserId = Guid.Parse("1BE5A09D-2240-43CC-8376-5944631D2ED3");

            var history = await _submissionService.GetMySubmissionsAsync(currentUserId);
            return Ok(new ApiResponse<IEnumerable<SubmissionHistoryDto>>
            {
                Message = "Lấy lịch sử nộp bài thành công",
                Data = history
            });
        }
    }
}
