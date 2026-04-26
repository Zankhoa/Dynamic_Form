using dynamic_form_system.Data;
using dynamic_form_system.DTOs.Requests;
using dynamic_form_system.Interface;
using dynamic_form_system.Interface.Validate;
using dynamic_form_system.Middlewares;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace dynamic_form_system.Validation
{
    public class SubmissionValidate : ISubmissionValidate
    {
        private readonly IFormRepository _formRepository;
        private readonly IFieldRepository _fieldRepository;

        public SubmissionValidate(IFormRepository formRepository, IFieldRepository fieldRepository)
        {
            _formRepository = formRepository;
            _fieldRepository = fieldRepository;
        }

        public async Task ValidateForSubmitAsync(Guid formId, SubmitFormRequestDto request)
        {
            // 1. Kiểm tra Form
            var form = await _formRepository.GetFormByIdAsync(formId);
            if (form == null || form.Status != "Active")
            {
                throw new KeyNotFoundException("Form không tồn tại hoặc hiện đang bị khóa.");
            }

            // 2. Validate cục JSON cơ bản
            if (string.IsNullOrWhiteSpace(request.Data))
            {
                throw new ArgumentException("Dữ liệu nộp lên không được để trống.");
            }

            JsonDocument jsonDoc;
            try { jsonDoc = JsonDocument.Parse(request.Data); }
            catch (JsonException)
            {
                throw new ArgumentException("Định dạng dữ liệu không phải JSON hợp lệ.");
            }

            // 3. Lấy cấu hình các field và soi chiếu
            var formFields = await _fieldRepository.GetListFieldByFormIdAsync(formId);
            var root = jsonDoc.RootElement;
            var errors = new Dictionary<string, string>();

            foreach (var field in formFields)
            {
                bool hasValue = root.TryGetProperty(field.Label, out JsonElement element);
                string stringValue = hasValue ? element.ToString() : null;
                bool isEmpty = string.IsNullOrWhiteSpace(stringValue);

                if (isEmpty)
                {
                    errors.Add(field.Label, $"Trường '{field.Label}' không được để trống.");
                    continue;
                }

                // Check Config (Regex, Min, Max...)
                if (!isEmpty)
                {
                    ValidateFieldRules(field, stringValue, errors);
                }
            }

            // 4. Nếu gom được bất kỳ lỗi nào -> Ném bom ra cho Middleware chụp!
            if (errors.Count > 0)
            {
                throw new ValidationAppException("Dữ liệu nộp lên không hợp lệ.", errors);
            }
        }

        private void ValidateFieldRules(FormField field, string stringValue, Dictionary<string, string> errors)
        {
            JsonDocument configDoc = null;
            if (!string.IsNullOrWhiteSpace(field.Configuration) && field.Configuration != "{}")
            {
                try { configDoc = JsonDocument.Parse(field.Configuration); }
                catch { return; }
            }
            var configRoot = configDoc?.RootElement;

            switch (field.FieldType.ToLower())
            {
                case "number":
                    if (!decimal.TryParse(stringValue, out decimal numValue))
                    {
                        errors.Add(field.Label, $"'{field.Label}' phải là con số.");
                    }
                    else if (configRoot.HasValue)
                    {
                        if (configRoot.Value.TryGetProperty("min", out JsonElement minEl) && minEl.TryGetDecimal(out decimal min) && numValue < min)
                            errors.Add(field.Label, $"'{field.Label}' phải >= {min}.");
                        if (configRoot.Value.TryGetProperty("max", out JsonElement maxEl) && maxEl.TryGetDecimal(out decimal max) && numValue > max)
                            errors.Add(field.Label, $"'{field.Label}' phải <= {max}.");
                    }
                    break;

                case "text":
                    if (configRoot.HasValue)
                    {
                        if (configRoot.Value.TryGetProperty("maxLength", out JsonElement maxLenEl) && maxLenEl.TryGetInt32(out int maxLen) && stringValue.Length > maxLen)
                            errors.Add(field.Label, $"'{field.Label}' không vượt quá {maxLen} ký tự.");

                        if (configRoot.Value.TryGetProperty("pattern", out JsonElement patternEl))
                        {
                            var regexPattern = patternEl.GetString();
                            if (!string.IsNullOrEmpty(regexPattern) && !Regex.IsMatch(stringValue, regexPattern))
                            {
                                string customMsg = configRoot.Value.TryGetProperty("patternMessage", out JsonElement msgEl) ? msgEl.GetString() : "Sai định dạng.";
                                errors.Add(field.Label, $"'{field.Label}': {customMsg}");
                            }
                        }
                    }
                    break;
            }
        }
    }
}
