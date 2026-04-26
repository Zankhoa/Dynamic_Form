using dynamic_form_system.Data;
using dynamic_form_system.DTOs.Requests;
using dynamic_form_system.Interface;
using dynamic_form_system.Interface.Validate;
using dynamic_form_system.Validation;
using System.Text.Json;

namespace dynamic_form_system.Services
{
    public class FieldService : IFieldService
    {
        private readonly IFieldRepository _fieldRepository;
        private readonly IFieldValidate _fieldValidate;
        public FieldService(IFieldRepository fieldRepository , IFieldValidate fieldValidator) 
        {
            _fieldRepository = fieldRepository;
            _fieldValidate = fieldValidator;
        }

        public async Task<Guid> AddFieldAsync(Guid formId, CreateFormFieldDto request)
        {
            await _fieldValidate.ValidateForAddAsync(formId, request);
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

        public async Task UpdateFieldAsync(Guid formId, Guid fieldId, UpdateFieldDto request)
        {
            var existingField = await _fieldRepository.GetFieldAsync(formId, fieldId);

            //validate
            await _fieldValidate.ValidateForUpdateAsync(formId, request, existingField);
            existingField.Name = request.Name;
            existingField.Label = request.Label;
            existingField.FieldType = request.FieldType;
            existingField.DisplayOrder = request.DisplayOrder;
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

        //    // 1. Check xem field có tồn tại không trước khi xóa
        //    _fieldValidate.ValidateForDelete(existingField);

        //    // 2. Xóa
        //    await _fieldRepository.DeleteAsync(existingField);
        //}
    }
}
