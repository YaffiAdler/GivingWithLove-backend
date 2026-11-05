using Bl.Abstractions;
using Bl.Dtos.Common;
using Bl.Dtos.Eichud;
using Microsoft.AspNetCore.Mvc;
using Web.Errors;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EichudController : ControllerBase
    {
        private readonly IEichudService eichudService;

        public EichudController(IEichudService eichudService)
        {
            this.eichudService = eichudService;
        }

        // Paged. search: a number matches the code exactly, any other text is searched in the first and family names.
        // sort: name (default: family name, then first name) | code.
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagedResult<EichudDto>>> GetPage([FromQuery] PageQueryDto query, CancellationToken ct)
        {
            return await eichudService.GetPageAsync(query, ct);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EichudDto>> GetById(int id, CancellationToken ct)
        {
            EichudDto? eichud = await eichudService.GetByIdAsync(id, ct);
            return eichud == null ? NotFound() : eichud;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EichudDto>> Create([FromBody] CreateEichudDto eichud, CancellationToken ct)
        {
            EichudDto created = await eichudService.CreateAsync(eichud, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.EichudCode }, created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEichudDto eichud, CancellationToken ct)
        {
            if (eichud.EichudCode != id)
                return this.IdMismatchProblem(id, eichud.EichudCode);

            await eichudService.UpdateAsync(eichud, ct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await eichudService.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}
