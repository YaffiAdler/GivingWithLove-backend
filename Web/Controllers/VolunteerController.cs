using Bl.BLApi;
using Bl.BLModels;
using Bl.Dtos.Volunteer;
using Microsoft.AspNetCore.Mvc;
using Web.Errors;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteerController : ControllerBase
    {
        private readonly BlVolunteerInterface volunteerService;

        public VolunteerController(BlVolunteerInterface volunteerService)
        {
            this.volunteerService = volunteerService;
        }

        [HttpGet]
        public async Task<ActionResult<List<BLVolunteerModel>>> GetAll(CancellationToken ct)
        {
            return await volunteerService.GetAllAsync(ct);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BLVolunteerModel>> GetById(int id, CancellationToken ct)
        {
            BLVolunteerModel? volunteer = await volunteerService.GetByIdAsync(id, ct);
            return volunteer == null ? NotFound() : volunteer;
        }

        [HttpPost]
        public async Task<ActionResult<BLVolunteerModel>> Create([FromBody] BLVolunteerModel volunteer, CancellationToken ct)
        {
            BLVolunteerModel created = await volunteerService.CreateAsync(volunteer, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.VolunteerCode }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BLVolunteerModel volunteer, CancellationToken ct)
        {
            if (volunteer.VolunteerCode != id)
                return this.IdMismatchProblem(id, volunteer.VolunteerCode);

            await volunteerService.UpdateAsync(volunteer, ct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await volunteerService.DeleteAsync(id, ct);
            return NoContent();
        }

        [HttpPut("domains")]
        public async Task<IActionResult> UpdateDomains([FromBody] UpdateVolunteerDomainsDto domains, CancellationToken ct)
        {
            await volunteerService.UpdateVolunteerDomainsAsync(domains, ct);
            return NoContent();
        }
    }
}
