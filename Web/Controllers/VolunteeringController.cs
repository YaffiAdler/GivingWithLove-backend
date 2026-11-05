using Bl.Abstractions;
using Bl.Dtos.Common;
using Bl.Dtos.Volunteering;
using Microsoft.AspNetCore.Mvc;
using Web.Errors;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteeringController : ControllerBase
    {
        private readonly IVolunteeringService volunteeringService;

        public VolunteeringController(IVolunteeringService volunteeringService)
        {
            this.volunteeringService = volunteeringService;
        }

        // Paged. Optional filters: volunteerCode, projectCode, from, to (yyyy-MM-dd, inclusive).
        // sort: date (default, newest first) | volunteer | project.
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagedResult<VolunteeringDto>>> GetPage(
            [FromQuery] VolunteeringPageQueryDto query,
            [FromQuery] VolunteeringFilterDto filter,
            CancellationToken ct)
        {
            if (filter.From > filter.To)
            {
                return Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Invalid date range.",
                    detail: $"'from' ({filter.From:yyyy-MM-dd}) is later than 'to' ({filter.To:yyyy-MM-dd}).");
            }

            return await volunteeringService.GetPageAsync(query, filter, ct);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VolunteeringDto>> GetById(int id, CancellationToken ct)
        {
            VolunteeringDto? volunteering = await volunteeringService.GetByIdAsync(id, ct);
            return volunteering == null ? NotFound() : volunteering;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<VolunteeringDto>> Create([FromBody] CreateVolunteeringDto volunteering, CancellationToken ct)
        {
            VolunteeringDto created = await volunteeringService.CreateAsync(volunteering, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.VolunteeringCode }, created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateVolunteeringDto volunteering, CancellationToken ct)
        {
            if (volunteering.VolunteeringCode != id)
                return this.IdMismatchProblem(id, volunteering.VolunteeringCode);

            await volunteeringService.UpdateAsync(volunteering, ct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await volunteeringService.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}
