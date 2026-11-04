using Bl.BLApi;
using Bl.BLModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubProjectController : ControllerBase
    {

        private readonly BlSubProjectInterface subProjectService;
        public SubProjectController(BlSubProjectInterface subProjectService)
        {
            this.subProjectService = subProjectService;
        }
        // GET: api/<SubProjectController>
        [HttpGet]
        public Task<List<BlSubProjectModel>> Get()
        {
            return subProjectService.ReadAll();
        }

        // GET api/<SubProjectController>/5
        [HttpGet("{id}")]
        public Task<BlSubProjectModel> Get(int id)
        {
            return subProjectService.Read(id);

        }

        // POST api/<SubProjectController>
        [HttpPost]
        public bool Post(BlSubProjectModel s)
        {
            try
            {
                subProjectService.Create(s);
                return true;
            }
            catch { return false; }
        }

        // PUT api/<SubProjectController>/5
        [HttpPut("{id}")]
        public bool Put(int id, BlSubProjectModel s)
        {
            try
            {
                subProjectService.Update(s);
                return true;
            }
            catch { return false; }
        }

        // DELETE api/<SubProjectController>/5
        [HttpDelete("{id}")]
        public bool Delete(BlSubProjectModel s)
        {
            try
            {
                subProjectService.Delete(s);
                return true;
            }
            catch { return false; }
        }
    }
}
