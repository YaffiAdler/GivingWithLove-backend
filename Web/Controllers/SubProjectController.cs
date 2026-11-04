using Bl.BLApi;
using Bl.BLModels;
using Microsoft.AspNetCore.Mvc;
using Web.Errors;

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
        public async Task<ActionResult<List<BlSubProjectModel>>> GetAll(CancellationToken ct)
        {
            return await subProjectService.GetAllAsync(ct);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlSubProjectModel>> GetById(int id, CancellationToken ct)
        {
            BlSubProjectModel? subProject = await subProjectService.GetByIdAsync(id, ct);
            return subProject == null ? NotFound() : subProject;
        }

        [HttpPost]
        public async Task<ActionResult<BlSubProjectModel>> Create([FromBody] BlSubProjectModel subProject, CancellationToken ct)
        {
            BlSubProjectModel created = await subProjectService.CreateAsync(subProject, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.SubProjectCode }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BlSubProjectModel subProject, CancellationToken ct)
        {
            if (subProject.SubProjectCode != id)
                return this.IdMismatchProblem(id, subProject.SubProjectCode);

            await subProjectService.UpdateAsync(subProject, ct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await subProjectService.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}
