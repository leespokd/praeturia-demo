//using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using praetura_demo.Models.LoanApplications;
using praetura_demo.Services.Interfaces;

namespace Praetura_demo.Controllers
{
    //normally I would use API versioning and set route at controller level,
    //but in order to adhere to the demo, I will leave it out for now. It can be added in the future if needed.
    //[ApiVersion("1")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class LoanApplicationsController : ControllerBase
    {
        private readonly ILoanApplicationsService _loanApplicationsService;

        public LoanApplicationsController(ILoanApplicationsService loanApplicationsService)
        {
            _loanApplicationsService = loanApplicationsService;
        }
        
        //error handling in middleware

        [HttpGet("loan-applications/{id}")]
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

        [HttpPost("loan-applications")]
        public async Task<IActionResult> Create([FromBody] CreateLoanApplicationDto createLoanApplicationDto,
            CancellationToken ct)
        {
            var result = await _loanApplicationsService.CreateAsync(createLoanApplicationDto, ct);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
    }
}
