using Bl.BLApi;
using Bl.BLModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        
        // GET: api/<EichudController>
        [HttpGet]
        public Task<List<BlEichudModel>> Get()
        {
             return eichudService.ReadAll();
        }

        // GET api/<EichudController>/5
        [HttpGet("{id}")]
        public Task<BlEichudModel> Get(int id)
        {
            return eichudService.Read(id);
        }

        // POST api/<EichudController>
        [HttpPost]
        public void Post([FromBody] BlEichudModel value)
        {
            eichudService.Create(value);
        }

        // PUT api/<EichudController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] BlEichudModel value)
        {
            eichudService.Update(value);
        }

        // DELETE api/<EichudController>/5
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            try
            {
                BlEichudModel v = eichudService.ReadAll().Result.Find(e=> e.EichudCode == id);
                if (v == null)
                    throw new NullReferenceException("mmm");
                eichudService.Delete(v);
                return true;
            }
            catch(Exception e) { return false; }
        }

    }
}
