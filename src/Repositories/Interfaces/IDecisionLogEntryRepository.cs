using Praetura_demo.Entities;

namespace Praetura_demo.Repositories.Interfaces
{
    public interface IDecisionLogEntryRepository
    {
        IQueryable<DecisionLogEntry> GetAll();
        IQueryable<DecisionLogEntry> GetAllAsNoTracking();
        Task<DecisionLogEntry?> GetByIdAsync(Guid id);
        Task AddAsync(DecisionLogEntry decisionLogEntry, CancellationToken ct = default);
        Task UpdateAsync(DecisionLogEntry decisionLogEntry, CancellationToken ct = default);
        Task DeleteAsync(DecisionLogEntry decisionLogEntry, CancellationToken ct = default);
    }
}
