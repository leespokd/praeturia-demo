using praetura_demo.Entities;

namespace praetura_demo.Services.Interfaces
{
    public interface ILoanProcessingService
    {
        List<DecisionLogEntry> ProcessLoan(LoanApplication loan);
    }
}
