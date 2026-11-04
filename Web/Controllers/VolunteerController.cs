using Bl.BLApi;
using Bl.BLModels;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<List<BLVolunteerModel>> Get(CancellationToken ct)
        {
            return await volunteerService.GetAllAsync(ct);
        }

        [HttpGet("{id}")]
        public async Task<BLVolunteerModel?> Get(int id, CancellationToken ct)
        {
            return await volunteerService.GetByIdAsync(id, ct);
        }

        [HttpPost]
        public async Task<bool> Post(BLVolunteerModel volunteer, CancellationToken ct)
        {
            await volunteerService.CreateAsync(volunteer, ct);
            return true;
        }

        [HttpPut("{id}")]
        public async Task<bool> Put(int id, BLVolunteerModel volunteer, CancellationToken ct)
        {
            try
            {
                await volunteerService.UpdateAsync(volunteer, ct);
                return true;
            }
            catch { return false; }
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id, CancellationToken ct)
        {
            try
            {
                await volunteerService.DeleteAsync(id, ct);
                return true;
            }
            catch { return false; }
        }

        [HttpPut("domains")]
        public async Task<IActionResult> UpdateDomains([FromBody] BLVolunteerModel volunteer, CancellationToken ct)
        {
            await volunteerService.UpdateVolunteerDomainsAsync(volunteer, ct);
            return Ok();
        }
    }
}
