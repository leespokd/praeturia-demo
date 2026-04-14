using System.ComponentModel.DataAnnotations.Schema;

namespace Praetura_demo.Entities
{
    public class LoanApplication
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        public decimal MonthlyIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal RequestedAmount { get; set; }
        public int TermMonths { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }

        public ICollection<DecisionLogEntry> DecisionLogEntries { get; set; } = new List<DecisionLogEntry>();
    }
}
