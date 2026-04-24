using dynamic_form_system.DTOs.Requests;
using dynamic_form_system.Entities;
using dynamic_form_system.Interface;
using System.Text.Json;

namespace dynamic_form_system.Services
{
    public class FieldService : IFieldService
    {
        private readonly IFieldRepository _fieldRepository;
        public FieldService(IFieldRepository fieldRepository) 
        {
            _fieldRepository = fieldRepository;
        }

        public async Task<Guid> AddFieldAsync(Guid formId, CreateFormFieldDto request)
        {
            if (!await _fieldRepository.FormExistsAsync(formId))
            {
                throw new Exception("Form không tồn tại.");
            }
            if (await _fieldRepository.FieldNameExistsAsync(formId, request.Name))
            {
                throw new Exception($"Field name '{request.Name}' đã tồn tại trong form này.");
            }
            if (!IsValidJson(request.Configuration))
            {
                throw new Exception("Configuration phải là định dạng JSON hợp lệ.");
            }

            var newField = new FormField
            {
                Id = Guid.NewGuid(),
                FormId = formId,
                Name = request.Name,
                Label = request.Label,
                FieldType = request.FieldType,
                DisplayOrder = request.DisplayOrder,
                IsRequired = request.IsRequired,
                Configuration = request.Configuration,
                CreatedAt = DateTime.UtcNow
            };

            await _fieldRepository.AddFieldAsync(newField);
            return newField.Id;
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

        public async Task UpdateFieldAsync(Guid formId, Guid fieldId, UpdateFieldDto request)
        {
            var existingField = await _fieldRepository.GetFieldAsync(formId, fieldId);
            if (existingField == null)
            {
                throw new Exception("Field không tồn tại trong form này.");
            }

            // 2. Kiểm tra trùng Name (Key của JSON) - bỏ qua chính nó
            if (!existingField.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase))
            {
                bool isDuplicate = await _fieldRepository.FieldNameExistsAsync(formId, request.Name);
                if (isDuplicate)
                {
                    throw new Exception($"Thuộc tính '{request.Name}' đã tồn tại trong form.");
                }
            }
            existingField.Name = request.Name;
            existingField.Label = request.Label;
            existingField.FieldType = request.FieldType;
            existingField.IsRequired = request.IsRequired;
            existingField.Configuration = request.Configuration;
            await _fieldRepository.Update(existingField);
        }

        public async Task ReorderFieldsAsync(Guid formId, ReorderFieldsRequest request)
        {
            
            var dbFields = await _fieldRepository.GetListFieldByFormIdAsync(formId);
            var fieldsToUpdate = new List<FormField>();
            foreach (var item in request.Fields)
            {
                var existingField = dbFields.FirstOrDefault(f => f.Id == item.Id);
                if (existingField != null && existingField.DisplayOrder != item.DisplayOrder)
                {
                    existingField.DisplayOrder = item.DisplayOrder;
                    fieldsToUpdate.Add(existingField);
                }
            }
            if (fieldsToUpdate.Any())
            {
                await _fieldRepository.UpdateRange(fieldsToUpdate);
            }
        }

        //public async Task DeleteFieldAsync(Guid formId, Guid fieldId)
        //{
        //    var existingField = await _fieldRepository.GetFieldAsync(formId, fieldId);
        //    if (existingField == null)
        //    {
        //        throw new Exception("Field không tồn tại trong form này.");
        //    }
        //    await _fieldRepository.DeleteAsync(existingField);
        //}
    }
}
