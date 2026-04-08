using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using praetura_demo.Models.LoanApplications;
using praetura_demo.Services.Interfaces;

namespace praetura_demo.Controllers.Loan
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly ILoanApplicationsService _loanApplicationsService;

        public ApplicationsController(ILoanApplicationsService loanApplicationsService)
        {
            _loanApplicationsService = loanApplicationsService;
        }
        
        //error handling in middleware

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id,
            CancellationToken ct)
        {
            var result = await _loanApplicationsService.GetByIdAsync(id, ct);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLoanApplicationDto createLoanApplicationDto,
            CancellationToken ct)
        {
            var result = await _loanApplicationsService.CreateAsync(createLoanApplicationDto, ct);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
    }
}
