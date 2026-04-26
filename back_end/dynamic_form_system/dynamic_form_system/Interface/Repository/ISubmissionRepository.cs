

using dynamic_form_system.Data;

namespace dynamic_form_system.Repository
{
    public interface ISubmissionRepository
    {
        Task AddAsync(Submission submission);
        Task<IEnumerable<Submission>> GetByUserIdAsync(Guid? userId);
    }
}
