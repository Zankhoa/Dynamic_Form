using dynamic_form_system.DTOs.Requests;
using dynamic_form_system.Interface.Validate;

namespace dynamic_form_system.Validation
{
    public class FormValidate : IFormValidate
    {
        public void ValidateForCreate(CreateFormRequestDto request)
        {
            if (request.Fields != null && request.Fields.Any())
            {
                // Validate duplicate name
                var duplicateNames = request.Fields
                    .GroupBy(x => x.Name)
                    .Where(g => g.Count() > 1)
                    .Select(y => y.Key)
                    .ToList();

                if (duplicateNames.Any())
                {
                    throw new ArgumentException($"Các trường bị trùng lặp mã (Name): {string.Join(", ", duplicateNames)}");
                }
            }
        }

        public void CheckFormExists<T>(T existingForm)
        {
            if (existingForm == null)
            {
                throw new KeyNotFoundException("Form không tồn tại hoặc đã bị xóa.");
            }
        }
    }
}
