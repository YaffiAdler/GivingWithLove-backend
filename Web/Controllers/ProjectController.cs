using Bl.BLApi;
using Bl.BLModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {

        private readonly BlProjectInterface projectService;
        public ProjectController(BlProjectInterface projectService)
        {
            this.projectService = projectService;
        }
        // GET: api/<ProjectController>
        [HttpGet]
        public Task<List<BlProjectModel>> Get()
        {
            return projectService.ReadAll();
        }

        // GET api/<ProjectController>/5
        [HttpGet("{id}")]
        public Task<BlProjectModel> Get(int id)
        {
            return projectService.Read(id);
        }

        // POST api/<ProjectController>
        [HttpPost]
        public bool Post(BlProjectModel s)
        {
            try
            {
                projectService.Create(s);
                return true;
            }
            catch { return false; }
        }

        // PUT api/<ProjectController>/5
        [HttpPut("{id}")]
        public bool Put(int id, BlProjectModel s)
        {
            try
            {
                projectService.Update(s);
                return true;
            }
            catch { return false; }
        }

        // DELETE api/<ProjectController>/5
        [HttpDelete("{id}")]
        public bool Delete(BlProjectModel s)
        {
            try
            {
                projectService.Delete(s);
                return true;
            }
            catch { return false; }
        }
    }
}
