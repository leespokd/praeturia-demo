using Microsoft.Extensions.Logging;
using Praetoria_demo.Repositories.Interfaces;
using Praetoria_demo.Services.Interfaces;
using Praetoria_demo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Praetoria_demo.Entities;

namespace Praetoria_demo.Tests
{
    public class LoanProcessingServiceShould
    {
        private readonly ILoanProcessingService _service;
        private readonly ILoanEligibilityService _eligibilityService;

        public LoanProcessingServiceShould()
        {
            _eligibilityService = new LoanEligibilityService();
            _service = new LoanProcessingService(_eligibilityService);
        }

        [Theory]
        [InlineData(1999.99, 2000.00, 12, "Declined")]
        [InlineData(2000.00, 2000.00, 12, "Approved")]
        [InlineData(2000.01, 2000.00, 12, "Approved")]
        [InlineData(2000.00, 8000.01, 12, "Declined")]
        [InlineData(2000.00, 8000.00, 12, "Approved")]
        [InlineData(2000.00, 7999.99, 12, "Approved")]
        [InlineData(2000.00, 2000.00, 11, "Declined")]
        [InlineData(2000.00, 2000.00, 60, "Approved")]
        [InlineData(2000.00, 2000.00, 61, "Declined")]
        public void ProcessLoan(decimal monthlyIncome, decimal requestedAmount, int termInMonths, string expectedStatus)
        {
            // Arrange
            var loan = new LoanApplication
            {
                Id = Guid.NewGuid(),
                MonthlyIncome = monthlyIncome,
                RequestedAmount = requestedAmount,
                TermMonths = termInMonths,
                Status = "Pending",
                DecisionLogEntries = new List<DecisionLogEntry>()
            };

            // Act
            _service.ProcessLoan(loan);

            // Assert
            Assert.Equal(expectedStatus, loan.Status);
        }
    }
}
