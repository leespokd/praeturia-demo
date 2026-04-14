using Praetura_demo.Entities;
using Praetura_demo.Wrappers;

namespace Praetura_demo.Services.Interfaces
{
    public interface ILoanProcessingService
    {
        Result<List<DecisionLogEntry>> ProcessLoan(LoanApplication loan);
    }
}
