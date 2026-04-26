using dynamic_form_system.Data;
using dynamic_form_system.DTOs.Requests;
using dynamic_form_system.DTOs.Responses;
using dynamic_form_system.Interface;
using dynamic_form_system.Interface.Validate; // Thêm dòng này
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dynamic_form_system.Services
{
    public class FormService : IFormService
    {
        private readonly IFormRepository _formRepository;
        private readonly IFormValidate _formValidate; // Khai báo Validator

        public FormService(IFormRepository formRepository, IFormValidate formValidate)
        {
            _formRepository = formRepository;
            _formValidate = formValidate;
        }

        // get list form for admin
        public async Task<PagedResult<FormAdminListDto>> GetAllFormsAsync(int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var (totalRecords, data) = await _formRepository.GetAllFormsAsync(page, pageSize);

            return new PagedResult<FormAdminListDto>
            {
                TotalRecords = totalRecords,
                PageIndex = page,
                PageSize = pageSize,
                Items = data
            };
        }

        // create form with field 
        public async Task<Guid> CreateFormAsync(CreateFormRequestDto request)
        {
            _formValidate.ValidateForCreate(request);
            var formId = Guid.NewGuid();
            var newForm = new Form
            {
                Id = formId,
                Title = request.Title,
                Description = request.Description,
                UserId = Guid.Parse("1BE5A09D-2240-43CC-8376-5944631D2ED3"),
                DisplayOrder = request.DisplayOrder,
                Status = request.Status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            var newFields = request.Fields.Select(f => new FormField
            {
                Id = Guid.NewGuid(),
                FormId = formId,
                Label = f.Label,
                Name = f.Name,  
                FieldType = f.FieldType,
                DisplayOrder = f.DisplayOrder,
                IsRequired = f.IsRequired,
                Configuration = string.IsNullOrWhiteSpace(f.Configuration) ? "{}" : f.Configuration,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            }).ToList();

            await _formRepository.CreateFormWithFieldsAsync(newForm, newFields);
            return formId;
        }

        // get form detail by id
        public async Task<FormDetailDto> GetFormByIdAsync(Guid id)
        {
            var formDetail = await _formRepository.GetFormDetailByIdAsync(id);
            _formValidate.CheckFormExists(formDetail);
            return formDetail;
        }

        // update form by id
        public async Task UpdateFormAsync(Guid id, UpdateFormRequestDto request)
        {
            var existingForm = await _formRepository.GetFormByIdAsync(id);
            _formValidate.CheckFormExists(existingForm);
            existingForm.Title = request.Title;
            existingForm.Description = request.Description;
            existingForm.Status = request.Status;
            existingForm.UpdatedAt = DateTime.UtcNow;

            await _formRepository.UpdateFormAsync(existingForm);
        }

        public async Task DeleteFormAsync(Guid id)
        {
            var existingForm = await _formRepository.GetFormByIdAsync(id);
            _formValidate.CheckFormExists(existingForm);
            await _formRepository.DeleteFormAsync(existingForm);
        }

        public async Task<IEnumerable<FormAdminListDto>> GetActiveFormsAsync()
        {
            var data = await _formRepository.GetActiveFormsAsync();
            return data;
        }
    }
}