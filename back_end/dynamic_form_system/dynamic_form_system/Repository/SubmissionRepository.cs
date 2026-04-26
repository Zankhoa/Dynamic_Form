using dynamic_form_system.Data;
using Microsoft.EntityFrameworkCore;

namespace dynamic_form_system.Repository
{
    public class SubmissionRepository : ISubmissionRepository
    {
        private readonly AppDbContext _context;

        public SubmissionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Submission submission)
        {
            _context.Submissions.AddAsync(submission);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Submission>> GetByUserIdAsync(Guid? userId)
        {
            var data = await _context.Submissions.Include(s => s.Form).Where(s => s.UserId == userId).OrderByDescending(s => s.SubmittedAt).AsNoTracking().ToListAsync();
            return data;
        }
    }
}
