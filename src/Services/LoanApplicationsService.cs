using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Praetura_demo.Entities;
using Praetura_demo.Models.LoanApplications;
using Praetura_demo.Repositories.Interfaces;
using Praetura_demo.Services.Interfaces;

namespace Praetura_demo.Services
{
    public class LoanApplicationsService : ILoanApplicationsService
    {
        private readonly ILogger<LoanApplicationsService> _logger;
        private readonly ILoanApplicationsRepository _loanApplicationsRepository;
        private readonly IMapper _mapper;

        public LoanApplicationsService(ILogger<LoanApplicationsService> logger,
            ILoanApplicationsRepository loanApplicationsRepository,
            IMapper mapper)
        {
            _logger = logger;
            _loanApplicationsRepository = loanApplicationsRepository;
            _mapper = mapper;
        }

        public async Task<LoanApplicationDto?> GetByIdAsync(Guid id,
            CancellationToken ct)
        {
            var loanApplication = await _loanApplicationsRepository
                .GetAllAsNoTracking()
                .Where(c => c.Id == id)
                .Include(c => c.DecisionLogEntries)
                .FirstOrDefaultAsync(ct);

            if (loanApplication == null) return null;

            return _mapper.Map<LoanApplicationDto>(loanApplication);
        }

        public async Task<CreatedLoanApplicationResultDto> CreateAsync(CreateLoanApplicationDto createLoanApplicationDto,
            CancellationToken ct)
        {
            var entity = _mapper.Map<LoanApplication>(createLoanApplicationDto);

            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;

            await _loanApplicationsRepository.AddAsync(entity, ct);

            return new CreatedLoanApplicationResultDto
            {
                Id = entity.Id,
                Status = entity.Status,
                CreatedAt = entity.CreatedAt
            };
        }
    }
}
