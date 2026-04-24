using dynamic_form_system.Data;
using dynamic_form_system.DTOs.Responses;
using dynamic_form_system.Entities;
using Microsoft.EntityFrameworkCore;

namespace dynamic_form_system.Interface
{
    public interface IFieldRepository
    {
        Task AddFieldAsync(FormField field);
        Task<bool> FormExistsAsync(Guid formId);
        Task<bool> FieldNameExistsAsync(Guid formId, string fieldName);
        Task<FormField> GetFieldAsync(Guid formId, Guid fieldId);
        Task<IEnumerable<FormField>> GetListFieldByFormIdAsync(Guid formId);
        Task Update(FormField field);
        Task UpdateRange(IEnumerable<FormField> fields);
        //void Delete(FormField field);
    }
}
