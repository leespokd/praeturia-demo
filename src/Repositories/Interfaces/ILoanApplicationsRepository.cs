using Praetoria_demo.Entities;

namespace Praetoria_demo.Repositories.Interfaces
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
