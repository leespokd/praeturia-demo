using praetura_demo.Services;
using praetura_demo.Services.Interfaces;

namespace Praetoria_demo.Tests
{
    public class LoanEligibilityServiceShould
    {
        private readonly ILoanEligibilityService _eligibilityService;

        public LoanEligibilityServiceShould()
        {
            _eligibilityService = new LoanEligibilityService();
        }

        [Theory]
        [InlineData(1999.99, false)]
        [InlineData(2000.00, true)]
        [InlineData(2000.01, true)]
        public void MonthlyIncomeGreaterThanMinAllowed(decimal monthlyIncome, bool expected)
        {
            // Act
            var result = _eligibilityService.IsMonthlyIncomeGreaterThanMinAllowed(monthlyIncome);
            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(2000.00, 8000.01, false)]
        [InlineData(2000.00, 8000.00, true)]
        [InlineData(2000.00, 7999.99, true)]
        public void AmountToBorrowNotGreaterThanAllowed(decimal monthlyIncome, decimal requestedAmount, bool expected)
        {
            // Act
            var result = _eligibilityService.IsRequestedAmountWithinAllowedLimit(monthlyIncome, requestedAmount);
            // Assert
            Assert.Equal(expected, result);
        }
        [Theory]
        [InlineData(11, false)]
        [InlineData(12, true)]
        [InlineData(60, true)]
        [InlineData(61, false)]
        public void TermInMonthsWithinRange(int termInMonths, bool expected)
        {
            // Act
            var result = _eligibilityService.IsTermInMonthsWithinRange(termInMonths);
            // Assert
            Assert.Equal(expected, result);
        }
    }
}
