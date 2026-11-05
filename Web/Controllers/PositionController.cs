using Bl.Abstractions;
using Bl.Dtos.Position;
using Microsoft.AspNetCore.Mvc;
using Web.Errors;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly IPositionService positionService;

        public PositionController(IPositionService positionService)
        {
            this.positionService = positionService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<PositionDto>>> GetAll(CancellationToken ct)
        {
            return await positionService.GetAllAsync(ct);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PositionDto>> GetById(int id, CancellationToken ct)
        {
            PositionDto? position = await positionService.GetByIdAsync(id, ct);
            return position == null ? NotFound() : position;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PositionDto>> Create([FromBody] CreatePositionDto position, CancellationToken ct)
        {
            PositionDto created = await positionService.CreateAsync(position, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.PositionCode }, created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePositionDto position, CancellationToken ct)
        {
            if (position.PositionCode != id)
                return this.IdMismatchProblem(id, position.PositionCode);

            await positionService.UpdateAsync(position, ct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await positionService.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}
