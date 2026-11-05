using Bl.Abstractions;
using Bl.Dtos.VolunteerDomain;
using Microsoft.AspNetCore.Mvc;
using Web.Errors;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteerDomainController : ControllerBase
    {
        private readonly IVolunteerDomainService volunteerDomainService;

        public VolunteerDomainController(IVolunteerDomainService volunteerDomainService)
        {
            this.volunteerDomainService = volunteerDomainService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<VolunteerDomainDto>>> GetAll(CancellationToken ct)
        {
            return await volunteerDomainService.GetAllAsync(ct);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VolunteerDomainDto>> GetById(int id, CancellationToken ct)
        {
            VolunteerDomainDto? volunteerDomain = await volunteerDomainService.GetByIdAsync(id, ct);
            return volunteerDomain == null ? NotFound() : volunteerDomain;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<VolunteerDomainDto>> Create([FromBody] CreateVolunteerDomainDto volunteerDomain, CancellationToken ct)
        {
            VolunteerDomainDto created = await volunteerDomainService.CreateAsync(volunteerDomain, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.VolunteerDomainsCode }, created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateVolunteerDomainDto volunteerDomain, CancellationToken ct)
        {
            if (volunteerDomain.VolunteerDomainsCode != id)
                return this.IdMismatchProblem(id, volunteerDomain.VolunteerDomainsCode);

            await volunteerDomainService.UpdateAsync(volunteerDomain, ct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await volunteerDomainService.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}
