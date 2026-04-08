using System.ComponentModel.DataAnnotations.Schema;

namespace praetura_demo.Models.DecisionLogEntries
{
    public class DecisionLogEntryDto
    {
        public Guid Id { get; set; }
        public Guid LoanApplicationId { get; set; }
        public string RuleName { get; set; } = string.Empty;
        public bool Passed { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime EvaluatedAt { get; set; }
    }
}
