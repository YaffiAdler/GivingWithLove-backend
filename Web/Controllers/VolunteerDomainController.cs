using Bl.BLApi;
using Bl.BLModels;
// BlVolunteerDomainInterface lives in namespace Dal.Api until BE-04 fixes it.
using Dal.Api;
// BlVolunteerDomainModel lives in namespace Dal.Models until BE-04 fixes it.
using Dal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VolunteerDomainController : ControllerBase
    {
        private readonly BlVolunteerDomainInterface volunteerDomainService;
        public VolunteerDomainController(BlVolunteerDomainInterface volunteerDomainService)
        {
            this.volunteerDomainService = volunteerDomainService;
        }

        [HttpGet]
        public async Task<List<BlVolunteerDomainModel>> Get(CancellationToken ct)
        {
            return await volunteerDomainService.GetAllAsync(ct);
        }

        [HttpGet("{id}")]
        public async Task<BlVolunteerDomainModel?> Get(int id, CancellationToken ct)
        {
            return await volunteerDomainService.GetByIdAsync(id, ct);
        }

        [HttpPost]
        public async Task Post([FromBody] BlVolunteerDomainModel volunteerDomain, CancellationToken ct)
        {
            await volunteerDomainService.CreateAsync(volunteerDomain, ct);
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] BlVolunteerDomainModel volunteerDomain, CancellationToken ct)
        {
            await volunteerDomainService.UpdateAsync(volunteerDomain, ct);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id, CancellationToken ct)
        {
            await volunteerDomainService.DeleteAsync(id, ct);
        }
    }
}
