using Praetoria_demo.Models.LoanApplications;

namespace Praetoria_demo.Services.Interfaces
{
    public interface ILoanApplicationsService
    {
        Task<LoanApplicationDto?> GetByIdAsync(Guid id,
            CancellationToken ct);
        Task<CreatedLoanApplicationResultDto> CreateAsync(CreateLoanApplicationDto createLoanApplicationDto,
            CancellationToken ct);
    }
}
