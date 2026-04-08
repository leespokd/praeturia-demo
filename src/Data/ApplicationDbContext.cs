using Microsoft.EntityFrameworkCore;
using Praetoria_demo.Entities;

namespace Praetoria_demo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<LoanApplication> LoanApplications { get; set; }
        public DbSet<DecisionLogEntry> DecisionLogEntries { get; set; }

    }
}
