using System.ComponentModel.DataAnnotations;

namespace Praetura_demo.Models.LoanApplications
{
    public class CreateLoanApplicationDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Range(typeof(decimal), "0.01", "999999999999999999", ErrorMessage = "Requested amount must be zero or greater, and less than 999999999999999999")]//max values to be decided
        public decimal MonthlyIncome { get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "999999999999999999", ErrorMessage = "Requested amount must be zero or greater, and less than 999999999999999999")]//max values to be decided
        public decimal RequestedAmount { get; set; }

        [Required]
        [Range(1, 600, ErrorMessage = "Term must be one or greater, and less than 600 months ")]//max values to be decided, for now 50 years max (incase of rule change in code to allow longer terms)
        public int TermMonths { get; set; }
    }
}
