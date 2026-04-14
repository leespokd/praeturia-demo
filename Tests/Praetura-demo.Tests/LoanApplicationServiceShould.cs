using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MockQueryable;
using Moq;
using praetura_demo.Entities;
using praetura_demo.Models.LoanApplications;
using praetura_demo.Profiles;
using praetura_demo.Repositories.Interfaces;
using praetura_demo.Services;
using praetura_demo.Services.Interfaces;

namespace Praetoria_demo.Tests
{
    public class LoanApplicationServiceShould
    {
        private readonly ILoanApplicationsService _service;
        private readonly Mock<ILogger<LoanApplicationsService>> _logger;
        private readonly Mock<ILoanApplicationsRepository> _loanApplicationsRepository;

        public LoanApplicationServiceShould()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<LoanApplicationProfile>();
                cfg.AddProfile<DecisionLogEntryProfile>();
            }, NullLoggerFactory.Instance);

            IMapper mapper = config.CreateMapper();

            _logger = new Mock<ILogger<LoanApplicationsService>>();
            _loanApplicationsRepository = new Mock<ILoanApplicationsRepository>();
            _service = new LoanApplicationsService(_logger.Object, _loanApplicationsRepository.Object, mapper);

        }

        [Fact]
        public async Task GetByIdAsync_ReturnsLoanApplication()
        {
            // Arrange
            _loanApplicationsRepository.Reset();//
            var loanApplicationId = Guid.NewGuid();

            var expectedLoanApplications = new List<LoanApplication>
            {
                new LoanApplication
                {
                    Id = loanApplicationId,
                    MonthlyIncome = 5000,
                    RequestedAmount = 10000,
                    TermMonths = 24,
                    Status = "Approved",
                    DecisionLogEntries = new List<DecisionLogEntry>
                    {
                        new DecisionLogEntry
                        {
                            Id = Guid.NewGuid(),
                            LoanApplicationId = loanApplicationId,
                            Passed = true,
                            RuleName = "Monthly Income",
                            EvaluatedAt = DateTime.UtcNow
                        },
                        new DecisionLogEntry
                        {
                            Id = Guid.NewGuid(),
                            LoanApplicationId = loanApplicationId,
                            Passed = true,
                            RuleName = "Requested Amount",
                            EvaluatedAt = DateTime.UtcNow
                        },
                        new DecisionLogEntry
                        {
                            Id = Guid.NewGuid(),
                            LoanApplicationId = loanApplicationId,
                            Passed = true,
                            RuleName = "Term Months",
                            EvaluatedAt = DateTime.UtcNow
                        }
                    }
                }
            };


            var mockQueryable = expectedLoanApplications.BuildMock();

            _loanApplicationsRepository
                .Setup(x => x.GetAllAsNoTracking())
                .Returns(mockQueryable);

            // Act
            var result = await _service.GetByIdAsync(loanApplicationId, CancellationToken.None);

            var expectedLoanApplication = expectedLoanApplications.First();

            // Assert the loan application details
            Assert.NotNull(result);
            Assert.Equal(expectedLoanApplication.Id, result.Id);
            Assert.Equal(expectedLoanApplication.MonthlyIncome, result.MonthlyIncome);
            Assert.Equal(expectedLoanApplication.RequestedAmount, result.RequestedAmount);
            Assert.Equal(expectedLoanApplication.TermMonths, result.TermMonths);
            Assert.Equal(expectedLoanApplication.Status, result.Status);

            //Assert the decision log entries
            Assert.Equal(3, result.DecisionLogEntries.Count);
            Assert.Contains(result.DecisionLogEntries, x =>
                x.RuleName == "Monthly Income" && x.Passed);
            Assert.Contains(result.DecisionLogEntries, x =>
                x.RuleName == "Requested Amount" && x.Passed);
            Assert.Contains(result.DecisionLogEntries, x =>
                x.RuleName == "Term Months" && x.Passed);
        }

        [Fact]
        public async Task CreateAsync_CreatesLoanApplication_AndReturnsCreatedResult()
        {
            // Arrange
            var request = new CreateLoanApplicationDto
            {
                Name = "test_name",
                Email = "email@email.test",
                MonthlyIncome = 5000,
                RequestedAmount = 10000,
                TermMonths = 24
            };

            _loanApplicationsRepository
                .Setup(r => r.AddAsync(It.IsAny<LoanApplication>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal("Pending", result.Status);
            Assert.NotEqual(default, result.CreatedAt);
        }
    }
}
