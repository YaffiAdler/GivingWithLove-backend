using Bl.Abstractions;
using Bl.Dtos.Project;
using Microsoft.AspNetCore.Mvc;
using Web.Errors;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService projectService;

        public ProjectController(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ProjectDto>>> GetAll(CancellationToken ct)
        {
            return await projectService.GetAllAsync(ct);
        }

        // Departments -> projects -> sub-projects. A full list: the tables are small lookups.
        [HttpGet("tree")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<DepartmentNodeDto>>> GetTree(CancellationToken ct)
        {
            return await projectService.GetTreeAsync(ct);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProjectDto>> GetById(int id, CancellationToken ct)
        {
            ProjectDto? project = await projectService.GetByIdAsync(id, ct);
            return project == null ? NotFound() : project;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProjectDto>> Create([FromBody] CreateProjectDto project, CancellationToken ct)
        {
            ProjectDto created = await projectService.CreateAsync(project, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.ProjectCode }, created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectDto project, CancellationToken ct)
        {
            if (project.ProjectCode != id)
                return this.IdMismatchProblem(id, project.ProjectCode);

            await projectService.UpdateAsync(project, ct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await projectService.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}
