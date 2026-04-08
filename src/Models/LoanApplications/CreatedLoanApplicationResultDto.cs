using System.ComponentModel.DataAnnotations.Schema;

namespace praetura_demo.Models.LoanApplications
{
    public class CreatedLoanApplicationResultDto
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
