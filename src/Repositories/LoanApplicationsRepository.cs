using Microsoft.EntityFrameworkCore;
using Praetura_demo.Data;
using Praetura_demo.Entities;
using Praetura_demo.Repositories.Interfaces;

namespace Praetura_demo.Repositories
{
    public class LoanApplicationsRepository : ILoanApplicationsRepository
    {
        private readonly ApplicationDbContext _context;

        public LoanApplicationsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<LoanApplication> GetAll()
        {
            return _context.LoanApplications;
        }
        public IQueryable<LoanApplication> GetAllAsNoTracking()
        {
            return _context.LoanApplications.AsNoTracking();
        }
        public async Task<LoanApplication?> GetByIdAsync(Guid id)
        {
            return await _context.LoanApplications.FindAsync(id);
        }
        public async Task AddAsync(LoanApplication loanApplication, CancellationToken ct = default)
        {
            _context.LoanApplications.Add(loanApplication);
            await _context.SaveChangesAsync(ct);
        }
        public async Task UpdateAsync(LoanApplication loanApplication, CancellationToken ct = default)
        {
            _context.LoanApplications.Update(loanApplication);
            await _context.SaveChangesAsync(ct);
        }
        public async Task DeleteAsync(LoanApplication loanApplication, CancellationToken ct = default)
        {
            _context.LoanApplications.Remove(loanApplication);
            await _context.SaveChangesAsync(ct);
        }
    }
}
