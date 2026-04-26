using dynamic_form_system.DTOs.Requests;
using dynamic_form_system.DTOs.Responses;
using dynamic_form_system.Interface;
using Microsoft.AspNetCore.Mvc;

namespace dynamic_form_system.Controllers.Admin
{
    [ApiController]
    [Route("api/forms/{formId:guid}/fields")]
    public class FieldManagementController : ControllerBase
    {
        private readonly IFieldService _fieldService;

        public FieldManagementController(IFieldService fieldService)
        {
            _fieldService = fieldService;
        }

        // POST /api/forms/:id/fields - add field new 
        [HttpPost]
        public async Task<IActionResult> AddField([FromRoute] Guid formId, [FromBody] CreateFormFieldDto request)
        {
            var newFieldId = await _fieldService.AddFieldAsync(formId, request);
            return Ok(new ApiResponse<Guid> { Data = newFieldId, Message = "Thêm Field thành công" });
        }

        // PUT /api/forms/:id/fields/:fid - update field
        [HttpPut("{fid:guid}")]
        public async Task<IActionResult> UpdateField([FromRoute] Guid formId, [FromRoute] Guid fid, [FromBody] UpdateFieldDto request)
        {
            await _fieldService.UpdateFieldAsync(formId, fid, request);
            return Ok(new ApiResponse<object>
            {
                Message = "Cập nhật Field thành công"
            });
        }

        // update position of field
        [HttpPut("reorder")]
        public async Task<IActionResult> ReorderFields([FromRoute] Guid formId, [FromBody] ReorderFieldsRequest request)
        {
            if (request == null || request.Fields == null || request.Fields.Count == 0)
            {
                return BadRequest(new ApiResponse<object>
                {
                    // Success = false, 
                    Message = "Danh sách vị trí không hợp lệ."
                });
            }

            await _fieldService.ReorderFieldsAsync(formId, request);
            return Ok(new ApiResponse<object>
            {
                Message = "Cập nhật thứ tự hiển thị thành công."
            });
        }

        //// 3. DELETE /api/forms/:id/fields/:fid - Xóa field
        //[HttpDelete("{fid:guid}")]
        //public async Task<IActionResult> DeleteField([FromRoute] Guid formId, [FromRoute] Guid fid)
        //{
        //    await _fieldService.DeleteFieldAsync(formId, fid);
        //    return Ok(new ApiResponse<object> { Message = "Xóa Field thành công" });
        //}
    }
}
