using dynamic_form_system.Data;
using dynamic_form_system.DTOs.Requests;


namespace dynamic_form_system.Interface.Validate
{
    public interface IFieldValidate
    {
        Task ValidateForAddAsync(Guid formId, CreateFormFieldDto request);
        Task ValidateForUpdateAsync(Guid formId, UpdateFieldDto request, FormField existingField);
        void ValidateForDelete(FormField existingField);
    }
}
