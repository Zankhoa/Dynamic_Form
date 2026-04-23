using dynamic_form_system.Data;
using dynamic_form_system.DTOs.Responses;
using dynamic_form_system.Entities;
using dynamic_form_system.Interface;
using Microsoft.EntityFrameworkCore;

namespace dynamic_form_system.Repository
{
    public class FormRepository : IFormRepository
    {
        private readonly AppDbContext _context;
        public FormRepository(AppDbContext context)
        {
            _context = context;
        }

        //get all form for admin
        public async Task<(int TotalRecords, IEnumerable<FormAdminListDto> Data)> GetAllFormsAsync(int page, int pageSize)
        {
            var query = _context.Forms;
            var totalRecords = await query.CountAsync();
            var data = await query
                .OrderByDescending(f => f.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(f => new FormAdminListDto
                {
                    Id = f.Id,
                    Title = f.Title,
                    //DisplayOrder = f.DisplayOrder,
                    Status = f.Status,
                    CreatedAt = f.CreatedAt
                }).ToListAsync();
            return (totalRecords, data);
        }

        //creae new form with field
        public async Task<Guid> CreateFormWithFieldsAsync(Form form, List<FormField> fields)
        {
            await _context.Forms.AddAsync(form);
            await _context.FormFields.AddRangeAsync(fields);
            await _context.SaveChangesAsync();
            return form.Id;
        }

        //get form by form id
        public async Task<Form?> GetFormByIdAsync(Guid id)
        {
            return await _context.Forms.FirstOrDefaultAsync(f => f.Id == id);
        }

        //update form 
        public async Task UpdateFormAsync(Form form)
        {
            _context.Forms.Update(form);
            await _context.SaveChangesAsync();
        }

        public async Task<FormDetailDto?> GetFormDetailByIdAsync(Guid id)
        {
            var data = await _context.Forms.Where(f => f.Id == id).Select(f => new FormDetailDto
            {
                Id = f.Id,
                Title = f.Title,
                Description = f.Description,
                //DisplayOrder = f.DisplayOrder,
                Status = f.Status,
                CreatedAt = f.CreatedAt,
                UpdatedAt = f.UpdatedAt,

                Fields = _context.FormFields.Where(field => field.FormId == f.Id).OrderBy(field => field.DisplayOrder).Select(field => new FormFieldDto
                {
                    Id = field.Id,
                    Name = field.Name,
                    Label = field.Label,
                    FieldType = field.FieldType,
                    //DisplayOrder = field.DisplayOrder,
                    //IsRequired = field.IsRequired,
                    Configuration = field.Configuration
                }).ToList()
            }).FirstOrDefaultAsync();
            return data;
        }

        //delete form
        public async Task DeleteFormAsync(Form form)
        {
            _context.Forms.Remove(form);
            await _context.SaveChangesAsync();
        }
    }
}
