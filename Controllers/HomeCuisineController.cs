using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding; // for ModelStateDictionary
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

        #region CRUD -> READ -> GET LIST (returns list of items)

        // GET api/homecuisine
        [HttpGet] // for testing
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "HomeCuisine1", "HomeCuisine2", "HomeCuisine3" };
        }

        // GET api/homecuisine/measures
        [HttpGet("measures")]
        [ProducesResponseType(typeof(IEnumerable<Measure>), 200)]
        public async Task<IActionResult> GetMeasures()
        {
            var res = await context.Measure
            .Where(x => x.Active == true)
            .OrderBy(y=> y.Name)
            .ToListAsync();

            return Ok(res);
        }

        // GET api/homecuisine/ingredients
        [HttpGet("ingredients")]
        [ProducesResponseType(typeof(IEnumerable<Error>), 200)]
        public async Task<IActionResult> GetIngredients()
        {
            var res = await context.Ingredient
            .ToListAsync();
            
            return Ok(res);
        }

        // GET api/homecuisine/errors
        [HttpGet("errors")]
        [ProducesResponseType(typeof(IEnumerable<Error>), 200)]
        public async Task<IActionResult> GetErrors()
        {
            var res = await context.Error
            .ToListAsync();
            
            return Ok(res);
        }

        #endregion

        #region CRUD -> READ -> CHECK (returns true/false)

        // GET api/homecuisine/ingredient-exists/{IngredientName}
        [HttpGet("ingredient-exists/{name}")]
        [ProducesResponseType(typeof(IEnumerable<Boolean>), 200)]
        public async Task<IActionResult> CheckIngredientExists(string name)
        {
            if (String.IsNullOrWhiteSpace(name)) {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError("Name", "Ingredient name is null or white space.");
                return BadRequest(modelState);
            } else {
                string trimLowercaseName = name.Trim().ToLower();
                var res = await context.Ingredient.FirstOrDefaultAsync(item => item.Name.ToLower() == trimLowercaseName);
                
                return Ok( (res != null)); // true/false
            }
        }

        #endregion

        #region CRUD -> CREATE

        // POST api/homecuisine/ingredient
        [HttpPost]
        [Route("ingredient")]
        public async Task<IActionResult> CreateIngredient([FromBody]Ingredient model)
        {
            context.Ingredient.Add(model);
            await context.SaveChangesAsync();
            return Ok(model);
        }

        // POST api/homecuisine/error
        [HttpPost]
        [Route("error")]
        public async Task<IActionResult> CreateError([FromBody]Error model)
        {
            context.Error.Add(model);
            await context.SaveChangesAsync();
            return Ok(model);
        }

        #endregion
    }
}
