using dynamic_form_system.DTOs.Responses;
using dynamic_form_system.Entities;

namespace dynamic_form_system.Interface
{
    public interface IFormRepository
    {
        Task<(int TotalRecords, IEnumerable<FormAdminListDto> Data)> GetAllFormsAsync(int page, int pageSize);
        Task<Guid> CreateFormWithFieldsAsync(Form form, List<FormField> fields);
        Task<Form?> GetFormByIdAsync(Guid id);
        Task UpdateFormAsync(Form form);
        Task<FormDetailDto?> GetFormDetailByIdAsync(Guid id);
        Task DeleteFormAsync(Form form);

    }
}
