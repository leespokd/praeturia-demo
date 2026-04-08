using Microsoft.EntityFrameworkCore;
using Praetoria_demo.Data;
using Praetoria_demo.Services.Interfaces;

namespace Praetoria_demo.Services
{
    public sealed class LoanAssessmentBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<LoanAssessmentBackgroundService> _logger;

        public LoanAssessmentBackgroundService(
            IServiceScopeFactory scopeFactory,
            ILogger<LoanAssessmentBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Loan assessment worker started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var processor = scope.ServiceProvider.GetRequiredService<ILoanProcessingService>();

                    //would ideally use a more efficient approach here, but for simplicity we will just load 20 pending loans into memory
                    //in a real application we would want to use a more efficient approach, such as processing loans in batches or using a queue-based system
                    //would likely implement a step here if multiple workers to change the status of the loans to "Processing" to avoid multiple workers processing the same loans,
                    //but for simplicity we will assume only one worker is running

                    var pendingLoans = await db.LoanApplications
                        .Where(x => x.Status == "Pending")
                        .OrderBy(x => x.CreatedAt)//get oldest first
                        .Take(20)
                        .ToListAsync(stoppingToken);

                    foreach (var loan in pendingLoans)
                    {
                        var logs = processor.ProcessLoan(loan);
                        db.DecisionLogEntries.AddRange(logs);
                    }

                    if (pendingLoans.Count > 0)
                    {
                        await db.SaveChangesAsync(stoppingToken);
                        _logger.LogInformation("Processed {Count} loan applications processed", pendingLoans.Count);
                        _logger.LogInformation("Processed {Count} loan applications approved", pendingLoans.Count(c => c.Status == "Approved"));
                        _logger.LogInformation("Processed {Count} loan applications declined", pendingLoans.Count(c => c.Status == "Declined"));
                        _logger.LogInformation("Processed {Count} loan applications were not processed", pendingLoans.Count(c => c.Status == "Pending"));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while processing loan applications");
                }

                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
            }

            _logger.LogInformation("Loan assessment worker stopped");
        }
    }
}
