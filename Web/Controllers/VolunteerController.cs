using Bl.Abstractions;
using Bl.Dtos.Common;
using Bl.Dtos.Volunteer;
using Microsoft.AspNetCore.Mvc;
using Web.Errors;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteerController : ControllerBase
    {
        private readonly IVolunteerService volunteerService;

        public VolunteerController(IVolunteerService volunteerService)
        {
            this.volunteerService = volunteerService;
        }

        // Paged. search: a number matches the code exactly, any other text is searched in the first and family names.
        // sort: name (default: family name, then first name) | code.
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagedResult<VolunteerDto>>> GetPage([FromQuery] PageQueryDto query, CancellationToken ct)
        {
            return await volunteerService.GetPageAsync(query, ct);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VolunteerDetailsDto>> GetById(int id, CancellationToken ct)
        {
            VolunteerDetailsDto? volunteer = await volunteerService.GetByIdAsync(id, ct);
            return volunteer == null ? NotFound() : volunteer;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<VolunteerDetailsDto>> Create([FromBody] CreateVolunteerDto volunteer, CancellationToken ct)
        {
            VolunteerDetailsDto created = await volunteerService.CreateAsync(volunteer, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.VolunteerCode }, created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateVolunteerDto volunteer, CancellationToken ct)
        {
            if (volunteer.VolunteerCode != id)
                return this.IdMismatchProblem(id, volunteer.VolunteerCode);

            await volunteerService.UpdateAsync(volunteer, ct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await volunteerService.DeleteAsync(id, ct);
            return NoContent();
        }

        [HttpPut("domains")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDomains([FromBody] UpdateVolunteerDomainsDto domains, CancellationToken ct)
        {
            await volunteerService.UpdateVolunteerDomainsAsync(domains, ct);
            return NoContent();
        }
    }
}
