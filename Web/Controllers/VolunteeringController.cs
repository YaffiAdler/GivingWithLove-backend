using Bl.BLApi;
using Bl.BLModels;
// BlVolunteeringInterface lives in namespace Dal.Api until BE-04 fixes it.
using Dal.Api;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<List<BlVolunteeringModel>> Get(CancellationToken ct)
        {
            return await volunteeringService.GetAllAsync(ct);
        }

        [HttpGet("{id}")]
        public async Task<BlVolunteeringModel?> Get(int id, CancellationToken ct)
        {
            return await volunteeringService.GetByIdAsync(id, ct);
        }

        [HttpPost]
        public async Task Post([FromBody] BlVolunteeringModel volunteering, CancellationToken ct)
        {
            await volunteeringService.CreateAsync(volunteering, ct);
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] BlVolunteeringModel volunteering, CancellationToken ct)
        {
            await volunteeringService.UpdateAsync(volunteering, ct);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id, CancellationToken ct)
        {
            await volunteeringService.DeleteAsync(id, ct);
        }
    }
}
