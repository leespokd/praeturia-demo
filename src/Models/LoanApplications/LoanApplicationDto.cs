using praetura_demo.Entities;
using praetura_demo.Models.DecisionLogEntries;

namespace praetura_demo.Models.LoanApplications
{
    public class LoanApplicationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal MonthlyIncome { get; set; }
        public decimal RequestedAmount { get; set; }
        public int TermMonths { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }

        public List<DecisionLogEntryDto> DecisionLogEntries { get; set; } = new List<DecisionLogEntryDto>();
    }
}
