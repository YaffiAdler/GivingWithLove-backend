using Bl.Abstractions;
using Bl.Dtos.SubProject;
using Microsoft.AspNetCore.Mvc;
using Web.Errors;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubProjectController : ControllerBase
    {
        private readonly ISubProjectService subProjectService;

        public SubProjectController(ISubProjectService subProjectService)
        {
            this.subProjectService = subProjectService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<SubProjectDto>>> GetAll(CancellationToken ct)
        {
            return await subProjectService.GetAllAsync(ct);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SubProjectDto>> GetById(int id, CancellationToken ct)
        {
            SubProjectDto? subProject = await subProjectService.GetByIdAsync(id, ct);
            return subProject == null ? NotFound() : subProject;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SubProjectDto>> Create([FromBody] CreateSubProjectDto subProject, CancellationToken ct)
        {
            SubProjectDto created = await subProjectService.CreateAsync(subProject, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.SubProjectCode }, created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSubProjectDto subProject, CancellationToken ct)
        {
            if (subProject.SubProjectCode != id)
                return this.IdMismatchProblem(id, subProject.SubProjectCode);

            await subProjectService.UpdateAsync(subProject, ct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await subProjectService.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}
