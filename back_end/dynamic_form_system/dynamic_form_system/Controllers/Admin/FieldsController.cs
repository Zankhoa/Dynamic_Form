//using dynamic_form_system.DTOs.Responses;
//using Microsoft.AspNetCore.Mvc;

//namespace dynamic_form_system.Controllers.Admin
//{
//    [ApiController]
//    [Route("api/forms/{formId:guid}/fields")]
//    public class FieldsController : ControllerBase
//    {
//        private readonly IFieldService _fieldService;

//        public FieldsController(IFieldService fieldService)
//        {
//            _fieldService = fieldService;
//        }

//        // 1. POST /api/forms/:id/fields - Thêm field vào form
//        [HttpPost]
//        public async Task<IActionResult> AddField([FromRoute] Guid formId, [FromBody] CreateFieldDto request)
//        {
//            var newFieldId = await _fieldService.AddFieldAsync(formId, request);
//            return Ok(new ApiResponse<Guid> { Data = newFieldId, Message = "Thêm Field thành công" });
//        }

//        // 2. PUT /api/forms/:id/fields/:fid - Cập nhật field
//        [HttpPut("{fid:guid}")]
//        public async Task<IActionResult> UpdateField([FromRoute] Guid formId, [FromRoute] Guid fid, [FromBody] UpdateFieldDto request)
//        {
//            await _fieldService.UpdateFieldAsync(formId, fid, request);
//            return Ok(new ApiResponse<object> { Message = "Cập nhật Field thành công" });
//        }

//        // 3. DELETE /api/forms/:id/fields/:fid - Xóa field
//        [HttpDelete("{fid:guid}")]
//        public async Task<IActionResult> DeleteField([FromRoute] Guid formId, [FromRoute] Guid fid)
//        {
//            await _fieldService.DeleteFieldAsync(formId, fid);
//            return Ok(new ApiResponse<object> { Message = "Xóa Field thành công" });
//        }
//    }
//}
