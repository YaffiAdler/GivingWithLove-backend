using Bl.BLApi;
using Bl.BLModels;
// BlVolunteerDomainInterface lives in namespace Dal.Api until BE-04 fixes it.
using Dal.Api;
// BlVolunteerDomainModel lives in namespace Dal.Models until BE-04 fixes it.
using Dal.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VolunteerDomainController : ControllerBase
    {
        private readonly BlVolunteerDomainInterface volunteerDomainService;
        public VolunteerDomainController(BlVolunteerDomainInterface volunteerDomainService)
        {
            this.volunteerDomainService = volunteerDomainService;
        }
        // GET: api/<VolunteeringController>
        [HttpGet]
        public Task<List<BlVolunteerDomainModel>> Get()
        {
            return volunteerDomainService.ReadAll();
        }

        // GET api/<VolunteeringController>/5
        [HttpGet("{id}")]
        public Task<BlVolunteerDomainModel> Get(int id)
        {
            return volunteerDomainService.Read(id);
        }

        // POST api/<VolunteeringController>
        [HttpPost]
        public void Post([FromBody] BlVolunteerDomainModel v)
        {
            volunteerDomainService.Create(v);
        }

        // PUT api/<VolunteeringController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] BlVolunteerDomainModel v)
        {
            volunteerDomainService.Update(v);

        }

        // DELETE api/<VolunteeringController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            volunteerDomainService.Delete(Get(id).Result);
        }
    }
}
