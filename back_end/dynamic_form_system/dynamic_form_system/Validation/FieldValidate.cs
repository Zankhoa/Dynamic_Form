using dynamic_form_system.Data;
using dynamic_form_system.DTOs.Requests;
using dynamic_form_system.Interface;
using dynamic_form_system.Interface.Validate;
using System.Text.Json;

namespace dynamic_form_system.Validation
{
    public class FieldValidate : IFieldValidate
    {
        private readonly IFieldRepository _fieldRepository;
        public FieldValidate(IFieldRepository fieldRepository)
        {
            _fieldRepository = fieldRepository;
        }

        public async Task ValidateForAddAsync(Guid formId, CreateFormFieldDto request)
        {
            if (!await _fieldRepository.FormExistsAsync(formId))
            {
                throw new KeyNotFoundException("Form không tồn tại.");
            }

            if (await _fieldRepository.FieldNameExistsAsync(formId, request.Name))
            {
                throw new ArgumentException($"Thuộc tính '{request.Name}' đã tồn tại trong form này.");
            }

            if (!IsValidJson(request.Configuration))
            {
                throw new ArgumentException("Configuration phải là định dạng JSON hợp lệ.");
            }
        }

        public async Task ValidateForUpdateAsync(Guid formId, UpdateFieldDto request, FormField existingField)
        {
            if (existingField == null)
            {
                throw new KeyNotFoundException("Field không tồn tại trong form này.");
            }

            if (!existingField.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase))
            {
                bool isDuplicate = await _fieldRepository.FieldNameExistsAsync(formId, request.Name);
                if (isDuplicate)
                {
                    throw new ArgumentException($"Thuộc tính '{request.Name}' đã tồn tại trong form.");
                }
            }

            if (!IsValidJson(request.Configuration))
            {
                throw new ArgumentException("Configuration phải là định dạng JSON hợp lệ.");
            }
        }

        public void ValidateForDelete(FormField existingField)
        {
            if (existingField == null)
            {
                throw new KeyNotFoundException("Field không tồn tại trong form này.");
            }

            // Nếu sau này có rule: "Không cho xóa Field nếu form đã có người nộp bài", 
            // bạn sẽ viết thêm logic kiểm tra vào đây.
        }

        private bool IsValidJson(string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) return false;
            strInput = strInput.Trim();

            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) ||
                (strInput.StartsWith("[") && strInput.EndsWith("]")))
            {
                try
                {
                    JsonDocument.Parse(strInput);
                    return true;
                }
                catch (JsonException)
                {
                    return false;
                }
            }
            return false;
        }
    }
}
