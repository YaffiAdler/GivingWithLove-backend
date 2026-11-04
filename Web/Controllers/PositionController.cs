using Bl.BLApi;
using Bl.BLModels;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<List<BlPositionModel>> Get(CancellationToken ct)
        {
            return await positionService.GetAllAsync(ct);
        }

        [HttpGet("{id}")]
        public async Task<BlPositionModel?> Get(int id, CancellationToken ct)
        {
            return await positionService.GetByIdAsync(id, ct);
        }

        [HttpPost]
        public async Task Post([FromBody] BlPositionModel value, CancellationToken ct)
        {
            await positionService.CreateAsync(value, ct);
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] BlPositionModel value, CancellationToken ct)
        {
            await positionService.UpdateAsync(value, ct);
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id, CancellationToken ct)
        {
            try
            {
                await positionService.DeleteAsync(id, ct);
                return true;
            }
            catch { return false; }
        }
    }
}
