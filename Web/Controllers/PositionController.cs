using Bl.BLApi;
using Bl.BLModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        // GET: api/<PositionController>
        [HttpGet]

        public Task<List<BlPositionModel>> Get()
        {
            return positionService.ReadAll();
        }
        
        // GET api/<PositionController>/5
        [HttpGet("{id}")]
        public Task<BlPositionModel> Get(int id)
        {
            return positionService.Read(id);
        }

        // POST api/<PositionController>
        [HttpPost]
        public void Post([FromBody] BlPositionModel value)
        {
            positionService.Create(value);
        }
        
        // PUT api/<PositionController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] BlPositionModel value)
        {
            positionService.Update(value);
        }
        
        // DELETE api/<PositionController>/5
        [HttpDelete("{id}")]
        public bool Delete(BlPositionModel id)
        {
            try
            {
                positionService.Delete(id);
                return true;
            }
            catch { return false; }
        }
    }
}
