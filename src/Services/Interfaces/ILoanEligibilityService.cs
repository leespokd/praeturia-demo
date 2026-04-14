namespace Praetura_demo.Services.Interfaces
{
    public interface ILoanEligibilityService
    {
        bool IsMonthlyIncomeGreaterThanMinAllowed(decimal monthlyIncome);
        bool IsRequestedAmountWithinAllowedLimit(decimal monthlyIncome, decimal requestedAmount);
        bool IsTermInMonthsWithinRange(int termInMonths);
    }
}
