using Bl.BLApi;
using Bl.BLModels;
using Microsoft.AspNetCore.Mvc;
using Web.Errors;

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
        public async Task<ActionResult<List<BlProjectModel>>> GetAll(CancellationToken ct)
        {
            return await projectService.GetAllAsync(ct);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlProjectModel>> GetById(int id, CancellationToken ct)
        {
            BlProjectModel? project = await projectService.GetByIdAsync(id, ct);
            return project == null ? NotFound() : project;
        }

        [HttpPost]
        public async Task<ActionResult<BlProjectModel>> Create([FromBody] BlProjectModel project, CancellationToken ct)
        {
            BlProjectModel created = await projectService.CreateAsync(project, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.ProjectCode }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BlProjectModel project, CancellationToken ct)
        {
            if (project.ProjectCode != id)
                return this.IdMismatchProblem(id, project.ProjectCode);

            await projectService.UpdateAsync(project, ct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await projectService.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}
