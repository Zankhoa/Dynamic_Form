using dynamic_form_system.DTOs.Requests;
using dynamic_form_system.DTOs.Responses;

namespace dynamic_form_system.Interface
{
    public interface IFieldService
    {
        Task<Guid> AddFieldAsync(Guid formId, CreateFormFieldDto request);
        Task UpdateFieldAsync(Guid formId, Guid fieldId, UpdateFieldDto request);
        Task ReorderFieldsAsync(Guid formId, ReorderFieldsRequest request);
        //Task DeleteFieldAsync(Guid formId, Guid fieldId);
    }
}
