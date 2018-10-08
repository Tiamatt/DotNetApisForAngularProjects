using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using DotNetApisForAngularProjects.HomeCuisineDbModels;

namespace DotNetApisForAngularProjects.Controllers
{
    [EnableCors("SiteCorsPolicy")]
    [Route("api/homecuisine")]
    [ApiController]
    public class HomeCuisineController : ControllerBase
    {
        private readonly HomeCuisineDbContext context;
        public HomeCuisineController(HomeCuisineDbContext _context) {
            this.context = _context;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "valueHC7", "value2", "valuetrt3" };
        }

        // GET api/homecuisine/measure
        [HttpGet("measure")]
        [ProducesResponseType(typeof(IEnumerable<Measure>), 200)]
        public async Task<IActionResult> GetMeasures()
        {
            var res = await context.Measure.ToListAsync();

            return Ok(res);
        }
    }
}
