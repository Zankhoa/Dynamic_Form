using dynamic_form_system.Data;
using dynamic_form_system.DTOs.Requests;

namespace dynamic_form_system.Interface.Validate
{
    public interface ISubmissionValidate
    {
        void ValidateSubmission(Form form, SubmitFormRequestDto request);
    }
}
