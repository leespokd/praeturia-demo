using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Praetoria_demo.Entities
{
    public class DecisionLogEntry
    {
        public Guid Id { get; set; }
        [ForeignKey("LoanApplication")]
        public Guid LoanApplicationId { get; set; }
        public string RuleName { get; set; } = string.Empty;
        public bool Passed { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime EvaluatedAt { get; set; }

        public LoanApplication LoanApplication { get; set; } = null!;
    }
}
