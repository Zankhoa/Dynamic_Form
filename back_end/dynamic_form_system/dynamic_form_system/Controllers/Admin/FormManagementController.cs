using dynamic_form_system.DTOs.Requests;
using dynamic_form_system.DTOs.Responses;
using dynamic_form_system.Interface;
using dynamic_form_system.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace dynamic_form_system.Controllers.Admin
{
    [ApiController]
    [Route("api/forms")]
    public class FormManagementController : ControllerBase
    {
        private readonly IFormService _IformService;

        public FormManagementController(IFormService IformService)
        {
            _IformService = IformService;
        }

        // GET /api/forms -- get list data form for admin
        [HttpGet]
        public async Task<IActionResult> GetAllForms([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _IformService.GetAllFormsAsync(page, pageSize);
            return Ok(new ApiResponse<object>
            { 
              Data = result, 
              Message = "Thành công" 
            });
        }

        // POST /api/forms - create new form for admin
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Guid>>> CreateForms([FromBody] CreateFormRequestDto request, Guid userId)
        {
            try
            {
                var formId = await _IformService.CreateFormAsync(request,userId );
                var response = new ApiResponse<Guid>
                {
                    Success = true,
                    Message = "Tạo form và các trường dữ liệu thành công!",
                    Data = formId
                };
                return Ok(response); 
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<Guid>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        // GET /api/forms/:id - get 1 form detail by id
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<FormDetailDto>>> GetFormById([FromRoute] Guid id)
        {
            try
            {
                var result = await _IformService.GetFormByIdAsync(id);
                var response = new ApiResponse<FormDetailDto>
                {
                    Success = true,
                    Message = "Lấy chi tiết form thành công",
                    Data = result
                };
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<FormDetailDto>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        //PUT /api/forms/:id - update information of form 
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateForm([FromRoute] Guid id, [FromBody] UpdateFormRequestDto request)
        {
            await _IformService.UpdateFormAsync(id, request);
            return Ok(new ApiResponse<object> { Message = "Cập nhật thành công" });
        }


        // DELETE /api/forms/:id - delete form 
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteForm([FromRoute] Guid id)
        {
            try
            {
                await _IformService.DeleteFormAsync(id);
                var response = new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Xóa form thành công!",
                    Data = true // Trả về true để báo hiệu đã xóa xong
                };
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                var errorResponse = new ApiResponse<bool>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = false
                };
                return NotFound(errorResponse);
            }
        }
    }
}
