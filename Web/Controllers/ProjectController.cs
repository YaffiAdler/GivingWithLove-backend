using Bl.BLApi;
using Bl.BLModels;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly BlProjectInterface projectService;
        public ProjectController(BlProjectInterface projectService)
        {
            this.projectService = projectService;
        }

        [HttpGet]
        public async Task<List<BlProjectModel>> Get(CancellationToken ct)
        {
            return await projectService.GetAllAsync(ct);
        }

        [HttpGet("{id}")]
        public async Task<BlProjectModel?> Get(int id, CancellationToken ct)
        {
            return await projectService.GetByIdAsync(id, ct);
        }

        [HttpPost]
        public async Task<bool> Post(BlProjectModel project, CancellationToken ct)
        {
            try
            {
                await projectService.CreateAsync(project, ct);
                return true;
            }
            catch { return false; }
        }

        [HttpPut("{id}")]
        public async Task<bool> Put(int id, BlProjectModel project, CancellationToken ct)
        {
            try
            {
                await projectService.UpdateAsync(project, ct);
                return true;
            }
            catch { return false; }
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id, CancellationToken ct)
        {
            try
            {
                await projectService.DeleteAsync(id, ct);
                return true;
            }
            catch { return false; }
        }
    }
}
