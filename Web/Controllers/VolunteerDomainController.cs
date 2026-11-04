// BlVolunteerDomainInterface lives in namespace Dal.Api until BE-04 fixes it.
using Dal.Api;
// BlVolunteerDomainModel lives in namespace Dal.Models until BE-04 fixes it.
using Dal.Models;
using Microsoft.AspNetCore.Mvc;
using Web.Errors;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteerDomainController : ControllerBase
    {
        private readonly BlVolunteerDomainInterface volunteerDomainService;

        public VolunteerDomainController(BlVolunteerDomainInterface volunteerDomainService)
        {
            this.volunteerDomainService = volunteerDomainService;
        }

        [HttpGet]
        public async Task<ActionResult<List<BlVolunteerDomainModel>>> GetAll(CancellationToken ct)
        {
            return await volunteerDomainService.GetAllAsync(ct);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlVolunteerDomainModel>> GetById(int id, CancellationToken ct)
        {
            BlVolunteerDomainModel? volunteerDomain = await volunteerDomainService.GetByIdAsync(id, ct);
            return volunteerDomain == null ? NotFound() : volunteerDomain;
        }

        [HttpPost]
        public async Task<ActionResult<BlVolunteerDomainModel>> Create([FromBody] BlVolunteerDomainModel volunteerDomain, CancellationToken ct)
        {
            BlVolunteerDomainModel created = await volunteerDomainService.CreateAsync(volunteerDomain, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.VolunteerDomainsCode }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BlVolunteerDomainModel volunteerDomain, CancellationToken ct)
        {
            if (volunteerDomain.VolunteerDomainsCode != id)
                return this.IdMismatchProblem(id, volunteerDomain.VolunteerDomainsCode);

            await volunteerDomainService.UpdateAsync(volunteerDomain, ct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await volunteerDomainService.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}
