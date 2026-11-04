using Bl.BLApi;
using Bl.BLModels;
using Microsoft.AspNetCore.Mvc;
using Web.Errors;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EichudController : ControllerBase
    {
        private readonly BlEichudInterface eichudService;

        public EichudController(BlEichudInterface eichudService)
        {
            this.eichudService = eichudService;
        }

        [HttpGet]
        public async Task<ActionResult<List<BlEichudModel>>> GetAll(CancellationToken ct)
        {
            return await eichudService.GetAllAsync(ct);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlEichudModel>> GetById(int id, CancellationToken ct)
        {
            BlEichudModel? eichud = await eichudService.GetByIdAsync(id, ct);
            return eichud == null ? NotFound() : eichud;
        }

        [HttpPost]
        public async Task<ActionResult<BlEichudModel>> Create([FromBody] BlEichudModel eichud, CancellationToken ct)
        {
            BlEichudModel created = await eichudService.CreateAsync(eichud, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.EichudCode }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BlEichudModel eichud, CancellationToken ct)
        {
            if (eichud.EichudCode != id)
                return this.IdMismatchProblem(id, eichud.EichudCode);

            await eichudService.UpdateAsync(eichud, ct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await eichudService.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}
