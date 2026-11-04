using Bl.BLApi;
using Bl.BLModels;
// BlVolunteeringInterface lives in namespace Dal.Api until BE-04 fixes it.
using Dal.Api;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteeringController : ControllerBase
    {
        private readonly BlVolunteeringInterface volunteeringService;
        public VolunteeringController(BlVolunteeringInterface volunteeringService)
        {
            this.volunteeringService = volunteeringService;
        }
        // GET: api/<VolunteeringController>
        [HttpGet]
        public Task<List<BlVolunteeringModel >> Get()
        {
          
                var volunteers = volunteeringService.ReadAll();
                if (volunteers == null )
                {
                    return ObjectNotFoundException();
                }
                return volunteers;
            }

        private async Task<List<BlVolunteeringModel>> ObjectNotFoundException()
        {
            throw new NotImplementedException();
        }       

        // GET api/<VolunteeringController>/5
        [HttpGet("{id}")]
        public Task<BlVolunteeringModel> Get(int id)
        {
            return volunteeringService.Read(id);
        }

        // POST api/<VolunteeringController>
        [HttpPost]
        public void Post([FromBody] BlVolunteeringModel v)
        {
            volunteeringService.Create(v);
        }

        // PUT api/<VolunteeringController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] BlVolunteeringModel v)
        {
            volunteeringService.Update(v);

        }

        // DELETE api/<VolunteeringController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            volunteeringService.Delete(Get(id).Result);
        }
    }
}
