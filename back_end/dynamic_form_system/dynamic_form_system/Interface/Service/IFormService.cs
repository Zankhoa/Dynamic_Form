using dynamic_form_system.DTOs.Requests;
using dynamic_form_system.DTOs.Responses;

namespace dynamic_form_system.Interface
{
    public interface IFormService
    {
        Task<PagedResult<FormAdminListDto>> GetAllFormsAsync(int page, int pageSize);
        Task<Guid> CreateFormAsync(CreateFormRequestDto request);
        Task<FormDetailDto> GetFormByIdAsync(Guid id);
        Task UpdateFormAsync(Guid id, UpdateFormRequestDto request);
        Task DeleteFormAsync(Guid id);
        Task<IEnumerable<FormAdminListDto>> GetActiveFormsAsync();
    }
}
