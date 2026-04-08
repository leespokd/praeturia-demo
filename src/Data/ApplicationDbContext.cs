using Microsoft.EntityFrameworkCore;
using praetura_demo.Entities;

namespace praetura_demo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<LoanApplication> LoanApplications { get; set; }
        public DbSet<DecisionLogEntry> DecisionLogEntries { get; set; }

    }
}
