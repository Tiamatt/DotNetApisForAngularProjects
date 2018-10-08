using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using DotNetApisForAngularProjects.PapaJohnsCloneDbModels;

namespace DotNetApisForAngularProjects.Controllers
{
    [EnableCors("SiteCorsPolicy")]
    [Route("api/ppjc")]
    [ApiController]
    public class PapaJohnsCloneController : ControllerBase
    {
        private readonly PapaJohnsCloneDbContext context;
        public PapaJohnsCloneController(PapaJohnsCloneDbContext _context) {
            this.context = _context;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "valuePPj7", "value2", "valuetrt3" };
        }

        // GET api/ppjc/items
        [HttpGet("items")]
        [ProducesResponseType(typeof(IEnumerable<Item>), 200)]
        public async Task<IActionResult> GetMeasures()
        {
            var res = await context.Item.ToListAsync();

            return Ok(res);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "valudfdfe";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
