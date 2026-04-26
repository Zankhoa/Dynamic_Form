using dynamic_form_system.DTOs.Requests;
using dynamic_form_system.DTOs.Responses;

namespace dynamic_form_system.Interface
{
    public interface ISubmissionService
    {
        Task<dynamic_form_system.Validation.SubmissionValidate> SubmitAsync(Guid formId, Guid? userId, SubmitFormRequestDto request);

        Task<IEnumerable<SubmissionHistoryDto>> GetMySubmissionsAsync(Guid? userId);
    }
}
