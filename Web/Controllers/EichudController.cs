using Bl.BLApi;
using Bl.BLModels;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<List<BlEichudModel>> Get(CancellationToken ct)
        {
            return await eichudService.GetAllAsync(ct);
        }

        [HttpGet("{id}")]
        public async Task<BlEichudModel?> Get(int id, CancellationToken ct)
        {
            return await eichudService.GetByIdAsync(id, ct);
        }

        [HttpPost]
        public async Task Post([FromBody] BlEichudModel value, CancellationToken ct)
        {
            await eichudService.CreateAsync(value, ct);
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] BlEichudModel value, CancellationToken ct)
        {
            await eichudService.UpdateAsync(value, ct);
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id, CancellationToken ct)
        {
            try
            {
                await eichudService.DeleteAsync(id, ct);
                return true;
            }
            catch { return false; }
        }
    }
}
