using Praetoria_demo.Entities;

namespace Praetoria_demo.Services.Interfaces
{
    public interface ILoanProcessingService
    {
        List<DecisionLogEntry> ProcessLoan(LoanApplication loan);
    }
}
