using Microsoft.EntityFrameworkCore;
using Praetoria_demo.Data;
using Praetoria_demo.Entities;
using Praetoria_demo.Repositories.Interfaces;

namespace Praetoria_demo.Repositories
{
    public class DecisionLogEntryRepository : IDecisionLogEntryRepository
    {
        private readonly ApplicationDbContext _context;

        public DecisionLogEntryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<DecisionLogEntry> GetAll()
        {
            return _context.DecisionLogEntries;
        }
        public IQueryable<DecisionLogEntry> GetAllAsNoTracking()
        {
            return _context.DecisionLogEntries.AsNoTracking();
        }
        public async Task<DecisionLogEntry?> GetByIdAsync(Guid id)
        {
            return await _context.DecisionLogEntries.FindAsync(id);
        }
        public async Task AddAsync(DecisionLogEntry decisionLogEntry, CancellationToken ct = default)
        {
            _context.DecisionLogEntries.Add(decisionLogEntry);
            await _context.SaveChangesAsync(ct);
        }
        public async Task UpdateAsync(DecisionLogEntry decisionLogEntry, CancellationToken ct = default)
        {
            _context.DecisionLogEntries.Update(decisionLogEntry);
            await _context.SaveChangesAsync(ct);
        }
        public async Task DeleteAsync(DecisionLogEntry decisionLogEntry, CancellationToken ct = default)
        {
            _context.DecisionLogEntries.Remove(decisionLogEntry);
            await _context.SaveChangesAsync(ct);
        }
    }
}
