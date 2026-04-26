using dynamic_form_system.DTOs.Requests;

namespace dynamic_form_system.Interface.Validate
{
    public interface ISubmissionValidate
    {
        Task ValidateForSubmitAsync(Guid formId, SubmitFormRequestDto request);
    }
}
