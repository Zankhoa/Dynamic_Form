using dynamic_form_system.Data;
using dynamic_form_system.DTOs.Requests;
using dynamic_form_system.Interface.Validate;
using dynamic_form_system.Middlewares;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace dynamic_form_system.Validation
{
    public class SubmissionValidate : ISubmissionValidate
    {
        public void ValidateSubmission(Form form, SubmitFormRequestDto request)
        {
            var errors = new Dictionary<string, string>();

            // 1. Kiểm tra chuỗi JSON đầu vào có bị rỗng không
            if (string.IsNullOrWhiteSpace(request.Data))
            {
                throw new ValidationAppException ( new Dictionary<string, string>
                { 
                    { "Data", "Dữ liệu nộp không được để trống." } 
                });
            }

            Dictionary<string, JsonElement> submittedData;
            try
            {
                // 2. Chuyển chuỗi JSON của Frontend thành dạng Dictionary để dễ bóc tách
                submittedData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(request.Data);
            }
            catch (JsonException)
            {
                throw new ValidationAppException(new Dictionary<string, string>
                { { "Data", "Định dạng JSON gửi lên không hợp lệ." } });
            }

            if (submittedData == null) submittedData = new Dictionary<string, JsonElement>();

            // 3. Duyệt qua TỪNG CÂU HỎI (Field) mà Form đã cấu hình trong Database
            foreach (var field in form.FormFields)
            {
                // Tìm xem người dùng có nộp trường này lên không
                bool hasValue = submittedData.TryGetValue(field.Name, out var element) &&
                                element.ValueKind != JsonValueKind.Null &&
                                element.ValueKind != JsonValueKind.Undefined &&
                                element.ToString() != ""; // Cả chuỗi rỗng cũng tính là không có

                // BƯỚC A: Kiểm tra trường Bắt buộc (IsRequired)
                if (field.IsRequired && !hasValue)
                {
                    errors.Add(field.Name, $"Trường '{field.Label}' là bắt buộc điền.");
                    continue; // Đã thiếu thì khỏi cần validate thêm logic bên dưới
                }

                // Nếu không bắt buộc và người dùng không điền thì an toàn bỏ qua
                if (!hasValue) continue;

                // 4. Đọc cấu hình logic (Configuration) của Field này
                Dictionary<string, JsonElement> config = null;
                if (!string.IsNullOrWhiteSpace(field.Configuration) && field.Configuration != "{}")
                {
                    try
                    {
                        config = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(field.Configuration);
                    }
                    catch { /* Bỏ qua nếu cấu hình trong DB bị lưu lỗi */ }
                }

                string stringValue = element.ToString();

                // BƯỚC B: Chạy rule Validate theo từng loại dữ liệu
                if (field.FieldType.ToLower() == "text")
                {
                    // Logic 1: Kiểm tra độ dài tối đa (maxLength)
                    if (config != null && config.ContainsKey("maxLength"))
                    {
                        int maxLength = config["maxLength"].GetInt32();
                        if (stringValue.Length > maxLength)
                        {
                            errors.Add(field.Name, $"Trường '{field.Label}' không được vượt quá {maxLength} ký tự.");
                        }
                    }

                    // Logic 2: Kiểm tra theo định dạng Regex (vd: Email, Số điện thoại)
                    if (config != null && config.ContainsKey("pattern"))
                    {
                        string pattern = config["pattern"].GetString();
                        if (!Regex.IsMatch(stringValue, pattern))
                        {
                            string msg = config.ContainsKey("patternMessage")
                                ? config["patternMessage"].GetString()
                                : $"Trường '{field.Label}' không đúng định dạng yêu cầu.";
                            errors.Add(field.Name, msg);
                        }
                    }
                }
                else if (field.FieldType.ToLower() == "number")
                {
                    // Phải là một con số hợp lệ mới cho qua
                    if (!decimal.TryParse(stringValue, out decimal numValue))
                    {
                        errors.Add(field.Name, $"Trường '{field.Label}' phải là một con số hợp lệ.");
                        continue;
                    }

                    // Logic 3: Kiểm tra giá trị nhỏ nhất (min)
                    if (config != null && config.ContainsKey("min"))
                    {
                        decimal min = config["min"].GetDecimal();
                        if (numValue < min) errors.Add(field.Name, $"Trường '{field.Label}' không được nhỏ hơn {min}.");
                    }

                    // Logic 4: Kiểm tra giá trị lớn nhất (max)
                    if (config != null && config.ContainsKey("max"))
                    {
                        decimal max = config["max"].GetDecimal();
                        if (numValue > max) errors.Add(field.Name, $"Trường '{field.Label}' không được lớn hơn {max}.");
                    }
                }
            }

            // 5. Nếu phát hiện ra dù chỉ 1 lỗi nhỏ, gom tất cả lại và ném thẳng ra Middleware!
            if (errors.Count > 0)
            {
                throw new ValidationAppException(errors);
            }
        }
    }
}