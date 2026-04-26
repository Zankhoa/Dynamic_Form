using dynamic_form_system.DTOs.Requests;

namespace dynamic_form_system.Interface.Validate
{
    public interface IFormValidate
    {
        void ValidateForCreate(CreateFormRequestDto request);
        void CheckFormExists<T>(T existingForm);
    }
}
