using dynamic_form_system.DTOs.Requests;
using dynamic_form_system.DTOs.Responses;
using dynamic_form_system.Entities;
using dynamic_form_system.Interface;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace dynamic_form_system.Services
{
    public class FormService :  IFormService
    {
        private readonly IFormRepository _formRepository;
        public FormService(IFormRepository formRepository)
        {
            _formRepository = formRepository;
        }

        //get list form for admin
        public async Task<PagedResult<FormAdminListDto>> GetAllFormsAsync(int page, int pageSize)
        {
            //validate page size
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100; 
            var (totalRecords, data) = await _formRepository.GetAllFormsAsync(page, pageSize);
            return new PagedResult<FormAdminListDto>
            {
                TotalRecords = totalRecords,
                PageIndex = page,
                PageSize = pageSize,
                Data = data
            };
        }

        //create form with field 
        public async Task<Guid> CreateFormAsync(CreateFormRequestDto request)
        {
            // validate dupliacate name
            var duplicateNames = request.Fields.GroupBy(x => x.Name).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
            if (duplicateNames.Any())
            {
                //handle error
                throw new ArgumentException($"Các trường bị trùng lặp mã (Name): {string.Join(", ", duplicateNames)}");
            }
            var formId = Guid.NewGuid();
            var newForm = new Form
            {
                Id = formId,
                Title = request.Title,
                Description = request.Description,
                //DisplayOrder = request.DisplayOrder,
                Status = request.Status,
                //IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            var newFields = request.Fields.Select(f => new FormField
            {
                Id = Guid.NewGuid(),
                FormId = formId,
                Name = f.Name,
                Label = f.Label,
                FieldType = f.FieldType,
                DisplayOrder = f.DisplayOrder,
                IsRequired = f.IsRequired,
                Configuration = string.IsNullOrWhiteSpace(f.Configuration) ? "{}" : f.Configuration,
                CreatedAt = DateTime.UtcNow
            }).ToList();
            await _formRepository.CreateFormWithFieldsAsync(newForm, newFields);
            return formId;
        }

        //get form detail by id
        public async Task<FormDetailDto> GetFormByIdAsync(Guid id)
        {
            var formDetail = await _formRepository.GetFormDetailByIdAsync(id);
            if (formDetail == null)
            {
                throw new KeyNotFoundException("Không tìm thấy Form, hoặc Form đã bị xóa.");
            }
            return formDetail;
        }

        //update form by id
        public async Task UpdateFormAsync(Guid id, UpdateFormRequestDto request)
        {
            // check form exist
            var existingForm = await _formRepository.GetFormByIdAsync(id);
            if (existingForm == null)
            {
                throw new KeyNotFoundException("Form không tồn tại hoặc đã bị xóa.");
            }
            existingForm.Title = request.Title;
            existingForm.Description = request.Description;
            //existingForm.DisplayOrder = request.DisplayOrder;
            existingForm.Status = request.Status;
            existingForm.UpdatedAt = DateTime.UtcNow;
            await _formRepository.UpdateFormAsync(existingForm);
        }

        public async Task DeleteFormAsync(Guid id)
        {
            var existingForm = await _formRepository.GetFormByIdAsync(id);
            if (existingForm == null)
            {
                throw new KeyNotFoundException("Form không tồn tại hoặc đã bị xóa trước đó.");
            }
            await _formRepository.DeleteFormAsync(existingForm);
        }
    }
}
