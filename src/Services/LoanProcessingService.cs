using praetura_demo.Entities;
using praetura_demo.Policies;
using praetura_demo.Services.Interfaces;

namespace praetura_demo.Services
{
    public class LoanProcessingService : ILoanProcessingService
    {
        private readonly ILoanEligibilityService _eligibilityService;

        public LoanProcessingService(ILoanEligibilityService eligibilityService)
        {
            _eligibilityService = eligibilityService;
        }

        public List<DecisionLogEntry> ProcessLoan(LoanApplication loan)
        {
            var incomePassed = _eligibilityService.IsMonthlyIncomeGreaterThanMinAllowed(loan.MonthlyIncome);
            var amountPassed = _eligibilityService.IsRequestedAmountWithinAllowedLimit(loan.MonthlyIncome, loan.RequestedAmount);
            var termPassed = _eligibilityService.IsTermInMonthsWithinRange(loan.TermMonths);

            var logs = new List<DecisionLogEntry>
            {
                CreateDecisionLogEntry(loan, incomePassed, "Monthly Income"),
                CreateDecisionLogEntry(loan, amountPassed, "Requested Amount"),
                CreateDecisionLogEntry(loan, termPassed, "Term")
            };

            var approved = incomePassed && amountPassed && termPassed;

            loan.Status = approved ? "Approved" : "Declined";
            loan.ReviewedAt = DateTime.UtcNow;

            return logs;
        }

        private static DecisionLogEntry CreateDecisionLogEntry(LoanApplication loan, bool passed, string name)
        {
            return new DecisionLogEntry
            {
                Id = Guid.NewGuid(),
                LoanApplicationId = loan.Id,
                RuleName = name,
                Passed = passed,
                Message = GetMessage(loan, name, passed),
                EvaluatedAt = DateTime.UtcNow
            };
        }

        private static string GetMessage(LoanApplication loan, string name, bool passed)
        {
            switch (name)
            {
                case "Monthly Income":

                    return passed
                    ? $"Monthly income {loan.MonthlyIncome} is above the minimum threshold of {EligibilityPolicyConstants.MinMonthlyIncomeGBP}"
                    : $"Monthly income {loan.MonthlyIncome} is below the minimum threshold of {EligibilityPolicyConstants.MinMonthlyIncomeGBP}";

                case "Requested Amount":
                    return passed
                        ? $"Requested amount {loan.RequestedAmount} is within the allowed limit based on monthly income"
                        : $"Requested amount {loan.RequestedAmount} exceeds the allowed limit based on monthly income";
                case "Term":
                    return passed
                        ? $"Requested term of {loan.TermMonths} months is within the allowed range"
                        : $"Requested term of {loan.TermMonths} months is outside the allowed range of {EligibilityPolicyConstants.MinTermMonths} to {EligibilityPolicyConstants.MaxTermMonths} months";

                default:
                    return string.Empty;
            }
        }
    }
}
