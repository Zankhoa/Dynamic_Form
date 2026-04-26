using dynamic_form_system.Data;
using dynamic_form_system.Interface;
using Microsoft.EntityFrameworkCore;

namespace dynamic_form_system.Repository
{
    public class FieldRepository : IFieldRepository
    {
        private readonly AppDbContext _context;

        public FieldRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<FormField>> GetListFieldByFormIdAsync(Guid formId)
        {
            return await _context.FormFields.Where(f => f.FormId == formId).AsNoTracking().ToListAsync();
        }
        public async Task<bool> FormExistsAsync(Guid formId)
        {
            return await _context.Forms.AnyAsync(f => f.Id == formId);
        }

        public async Task<bool> FieldNameExistsAsync(Guid formId, string fieldName)
        {
            return await _context.FormFields.AnyAsync(f => f.FormId == formId && f.Name == fieldName);
        }
        public async Task AddFieldAsync(FormField field)
        {
            _context.FormFields.Add(field);
            await _context.SaveChangesAsync();
        }

        public async Task<FormField> GetFieldAsync(Guid formId, Guid fieldId)
        {
            var data = await _context.FormFields.FirstOrDefaultAsync(f => f.FormId == formId && f.Id == fieldId);
            return data;
        }

        public async Task<IEnumerable<FormField>> GetEntitiesByFormIdAsync(Guid formId)
        {
            var data = await _context.FormFields.Where(f => f.FormId == formId).ToListAsync();
            return data;
        }

        public async Task Update(FormField field)
        {
            _context.FormFields.Update(field);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRange(IEnumerable<FormField> fields)
        {
            _context.FormFields.UpdateRange(fields);
            await _context.SaveChangesAsync();
        }

        //public void Delete(FormField field)
        //{
        //    _context.FormFields.Remove(field);
        //}
    }
}
