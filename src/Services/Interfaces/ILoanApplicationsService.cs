using Praetura_demo.Models.LoanApplications;

namespace Praetura_demo.Services.Interfaces
{
    public interface ILoanApplicationsService
    {
        Task<LoanApplicationDto?> GetByIdAsync(Guid id,
            CancellationToken ct);
        Task<CreatedLoanApplicationResultDto> CreateAsync(CreateLoanApplicationDto createLoanApplicationDto,
            CancellationToken ct);
    }
}
