using Bl.Dtos.Volunteering;
// BlVolunteeringInterface lives in namespace Dal.Api until BE-04 fixes it.
using Dal.Api;
using Microsoft.AspNetCore.Mvc;
using Web.Errors;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteeringController : ControllerBase
    {
        private readonly BlVolunteeringInterface volunteeringService;

        public VolunteeringController(BlVolunteeringInterface volunteeringService)
        {
            this.volunteeringService = volunteeringService;
        }

        // All filters are optional query parameters: volunteerCode, projectCode, from, to (yyyy-MM-dd, inclusive).
        [HttpGet]
        public async Task<ActionResult<List<VolunteeringDto>>> GetAll([FromQuery] VolunteeringFilterDto filter, CancellationToken ct)
        {
            if (filter.From > filter.To)
            {
                return Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Invalid date range.",
                    detail: $"'from' ({filter.From:yyyy-MM-dd}) is later than 'to' ({filter.To:yyyy-MM-dd}).");
            }

            return await volunteeringService.GetAllAsync(filter, ct);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VolunteeringDto>> GetById(int id, CancellationToken ct)
        {
            VolunteeringDto? volunteering = await volunteeringService.GetByIdAsync(id, ct);
            return volunteering == null ? NotFound() : volunteering;
        }

        [HttpPost]
        public async Task<ActionResult<VolunteeringDto>> Create([FromBody] CreateVolunteeringDto volunteering, CancellationToken ct)
        {
            VolunteeringDto created = await volunteeringService.CreateAsync(volunteering, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.VolunteeringCode }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateVolunteeringDto volunteering, CancellationToken ct)
        {
            if (volunteering.VolunteeringCode != id)
                return this.IdMismatchProblem(id, volunteering.VolunteeringCode);

            await volunteeringService.UpdateAsync(volunteering, ct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await volunteeringService.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}
