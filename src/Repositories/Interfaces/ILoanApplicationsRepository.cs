using Praetura_demo.Entities;

namespace Praetura_demo.Repositories.Interfaces
{
    public interface ILoanApplicationsRepository
    {
        IQueryable<LoanApplication> GetAll();
        IQueryable<LoanApplication> GetAllAsNoTracking();
        Task<LoanApplication?> GetByIdAsync(Guid id);
        Task AddAsync(LoanApplication loanApplication, CancellationToken ct = default);
        Task UpdateAsync(LoanApplication loanApplication, CancellationToken ct = default);
        Task DeleteAsync(LoanApplication loanApplication, CancellationToken ct = default);
    }
}
