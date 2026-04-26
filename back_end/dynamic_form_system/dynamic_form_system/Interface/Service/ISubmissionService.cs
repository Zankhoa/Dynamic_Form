using dynamic_form_system.DTOs.Requests;
using dynamic_form_system.DTOs.Responses;

namespace dynamic_form_system.Interface
{
    public interface ISubmissionService
    {
        Task SubmitAsync(Guid formId, SubmitFormRequestDto request);
        Task<IEnumerable<SubmissionHistoryDto>> GetMySubmissionsAsync(Guid? userId);
    }
}
