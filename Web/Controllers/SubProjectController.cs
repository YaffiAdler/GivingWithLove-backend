using Bl.BLApi;
using Bl.BLModels;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubProjectController : ControllerBase
    {
        private readonly BlSubProjectInterface subProjectService;
        public SubProjectController(BlSubProjectInterface subProjectService)
        {
            this.subProjectService = subProjectService;
        }

        [HttpGet]
        public async Task<List<BlSubProjectModel>> Get(CancellationToken ct)
        {
            return await subProjectService.GetAllAsync(ct);
        }

        [HttpGet("{id}")]
        public async Task<BlSubProjectModel?> Get(int id, CancellationToken ct)
        {
            return await subProjectService.GetByIdAsync(id, ct);
        }

        [HttpPost]
        public async Task<bool> Post(BlSubProjectModel subProject, CancellationToken ct)
        {
            try
            {
                await subProjectService.CreateAsync(subProject, ct);
                return true;
            }
            catch { return false; }
        }

        [HttpPut("{id}")]
        public async Task<bool> Put(int id, BlSubProjectModel subProject, CancellationToken ct)
        {
            try
            {
                await subProjectService.UpdateAsync(subProject, ct);
                return true;
            }
            catch { return false; }
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id, CancellationToken ct)
        {
            try
            {
                await subProjectService.DeleteAsync(id, ct);
                return true;
            }
            catch { return false; }
        }
    }
}
