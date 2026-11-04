using Bl.BLApi;
using Bl.BLModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteerController : ControllerBase
    {

        private readonly BlVolunteerInterface volunteerService;
        public VolunteerController(BlVolunteerInterface volunteerService)
        {
            this.volunteerService = volunteerService;
        }

        // GET: api/<VolunteerController>
        [HttpGet]
        public Task<List<BLVolunteerModel>> Get()
        {
            return volunteerService.ReadAll();
        }

        // GET api/<VolunteerController>/5
        [HttpGet("{id}")]
        public Task<BLVolunteerModel> Get(int id)
        {                   
            return volunteerService.Read(id);
        }

        // POST api/<VolunteerController>
        [HttpPost]
        public bool Post(BLVolunteerModel v)
        {
            try { 
                volunteerService.Create(v);
                return true; }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        // PUT api/<VolunteerController>/5
        [HttpPut("{id}")]
        public bool Put(int id, BLVolunteerModel v)
        {
            try
            {
                
                volunteerService.Update(v);
                return true;
            }
            catch { return false; }

        }

        // DELETE api/<VolunteerController>/5
        [HttpDelete("{id}")]
        public bool Delete(BLVolunteerModel v)
        {
            try
            {
                volunteerService.Delete(v);
                return true;
            }
            catch { return false; }
        }
        [HttpPut("domains")]
        public async Task<IActionResult> UpdateDomains([FromBody] BLVolunteerModel dto)
        {
            await volunteerService.UpdateVolunteerDomains(dto);
            return Ok();
        }

    }
}

