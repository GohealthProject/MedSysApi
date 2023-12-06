using MedSysApi.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MedSysApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class planComeparisonTotalController : ControllerBase
    {
        private readonly MedSysContext _context;

        public planComeparisonTotalController(MedSysContext context)
        {
            _context = context;
        }


        // GET: api/<planComeparisonTotal>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<planComeparisonTotal>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<planComeparisonTotal>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<planComeparisonTotal>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<planComeparisonTotal>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
