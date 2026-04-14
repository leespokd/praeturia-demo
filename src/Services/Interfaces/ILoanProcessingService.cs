using praetura_demo.Entities;
using Praetura_demo.Wrappers;

namespace praetura_demo.Services.Interfaces
{
    public interface ILoanProcessingService
    {
        Result<List<DecisionLogEntry>> ProcessLoan(LoanApplication loan);
    }
}
