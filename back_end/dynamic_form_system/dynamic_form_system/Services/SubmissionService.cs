using dynamic_form_system.Data;
using dynamic_form_system.DTOs.Requests;
using dynamic_form_system.DTOs.Responses;
using dynamic_form_system.Interface;
using dynamic_form_system.Interface.Validate;
using dynamic_form_system.Repository;
using System.Text.Json;

namespace dynamic_form_system.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly ISubmissionRepository _submissionRepository;
        private readonly IFormRepository _formRepository;
        private readonly ISubmissionValidate _submissionValidate;

        // Tiêm các dependency cần thiết vào
        public SubmissionService(
            ISubmissionRepository submissionRepository,
            IFormRepository formRepository,
            ISubmissionValidate submissionValidate)
        {
            _submissionRepository = submissionRepository;
            _formRepository = formRepository;
            _submissionValidate = submissionValidate;
        }

        public async Task SubmitAsync(Guid formId, SubmitFormRequestDto request)
        {
            var form = await _formRepository.GetFormByIdAsync(formId);
            if (form == null || form.Status != "Active")
            {
                throw new KeyNotFoundException("Form không tồn tại hoặc hiện đang bị đóng.");
            }
            _submissionValidate.ValidateSubmission(form, request);
            var submission = new Submission
            {
                Id = Guid.NewGuid(),
                FormId = formId,
                UserId = Guid.Parse("1BE5A09D-2240-43CC-8376-5944631D2ED3"),
                Data = request.Data,

                SubmittedAt = DateTime.UtcNow
            };

            await _submissionRepository.AddAsync(submission);
        }

        public async Task<IEnumerable<SubmissionHistoryDto>> GetMySubmissionsAsync(Guid? userId)
        {
            if (userId == null) return new List<SubmissionHistoryDto>();

            var submissions = await _submissionRepository.GetByUserIdAsync(userId.Value);
            var result = new List<SubmissionHistoryDto>();

            foreach (var s in submissions)
            {
                var dto = new SubmissionHistoryDto
                {
                    Id = s.Id,
                    FormTitle = s.Form?.Title,
                    SubmittedAt = s.SubmittedAt,
                    Value = s.Data,
                };
                Dictionary<string, JsonElement> parsedAnswers = new();
                if (!string.IsNullOrWhiteSpace(s.Data))
                {
                        parsedAnswers = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(s.Data);
                }
                if (s.Form != null && s.Form.FormFields != null)
                {
                    foreach (var field in s.Form.FormFields.OrderBy(f => f.DisplayOrder))
                    {
                        string answeredValue = "";
                        if (parsedAnswers.TryGetValue(field.Name, out var jsonValue))
                        {
                            answeredValue = jsonValue.ToString();
                        }
                    }
                }
                result.Add(dto);
            }
            return result.OrderByDescending(s => s.SubmittedAt).ToList();
        }

    }
}
