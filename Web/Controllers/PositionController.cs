using Bl.BLApi;
using Bl.BLModels;
using Microsoft.AspNetCore.Mvc;
using Web.Errors;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly BlPositionInterface positionService;

        public PositionController(BlPositionInterface positionService)
        {
            this.positionService = positionService;
        }

        [HttpGet]
        public async Task<ActionResult<List<BlPositionModel>>> GetAll(CancellationToken ct)
        {
            return await positionService.GetAllAsync(ct);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlPositionModel>> GetById(int id, CancellationToken ct)
        {
            BlPositionModel? position = await positionService.GetByIdAsync(id, ct);
            return position == null ? NotFound() : position;
        }

        [HttpPost]
        public async Task<ActionResult<BlPositionModel>> Create([FromBody] BlPositionModel position, CancellationToken ct)
        {
            BlPositionModel created = await positionService.CreateAsync(position, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.positionCode }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BlPositionModel position, CancellationToken ct)
        {
            if (position.positionCode != id)
                return this.IdMismatchProblem(id, position.positionCode);

            await positionService.UpdateAsync(position, ct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await positionService.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}
