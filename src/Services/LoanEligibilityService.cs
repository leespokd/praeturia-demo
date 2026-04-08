using praetura_demo.Policies;
using praetura_demo.Services.Interfaces;

namespace praetura_demo.Services
{
    public class LoanEligibilityService : ILoanEligibilityService
    {
        public bool IsRequestedAmountWithinAllowedLimit(decimal monthlyIncome, decimal requestedAmount)
        {
            return requestedAmount <= monthlyIncome * EligibilityPolicyConstants.MaxIncomeMultiplier;
        }

        public bool IsMonthlyIncomeGreaterThanMinAllowed(decimal monthlyIncome)
        {
            return monthlyIncome >= EligibilityPolicyConstants.MinMonthlyIncomeGBP;
        }

        public bool IsTermInMonthsWithinRange(int termInMonths)
        {
            return termInMonths >= EligibilityPolicyConstants.MinTermMonths && termInMonths <= EligibilityPolicyConstants.MaxTermMonths;
        }
    }
}
