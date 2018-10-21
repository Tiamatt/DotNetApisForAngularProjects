using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding; // for ModelStateDictionary
using Microsoft.AspNetCore.Http; // for image save
using DotNetApisForAngularProjects.HomeCuisineDbModels;
using DotNetApisForAngularProjects.HomeCuisineCustomModels;
using DotNetApisForAngularProjects.Helpers;

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

        
        // GET api/homecuisine/filters/{entityName}
        [HttpGet("filter/{entityName}")]
        [ProducesResponseType(typeof(IEnumerable<Error>), 200)]
        public async Task<IActionResult> GetFilter(string entityName)
        {
            // kali - refactor, use dynamic entity something like context.Set(typeof("Measure")) ...
            object res = null;
            switch(entityName) {
                case "measure":
                  res = await context.Measure
                    .Where(x => x.Active == true)
                    .Select(y => new FilterModel{
                        name = y.Name,
                        value = y.Id.ToString(),
                        selected = false,
                    })
                    .OrderBy(y=> y.name)
                    .ToListAsync();
                  break;
                case "ingredient":
                  res = await context.Ingredient
                    .Where(x => x.Active == true)
                    .Select(y => new FilterModel{
                        name = y.Name,
                        value = y.Id.ToString(),
                        selected = false,
                    })
                    .OrderBy(y=> y.name)
                    .ToListAsync();
                  break;
            }

            if(res ==null){
                NotFound();
            }

            return Ok(res);
        }


        [HttpGet("recipe/{id}")]
        [ProducesResponseType(typeof(RecipeModel), 200)]
        public async Task<IActionResult> GetRecipe(int id){
            RecipeModel recipeModel = new RecipeModel();

            // get Recipe
            Recipe recipe = await context.Recipe.Where(x => x.Id == id).FirstOrDefaultAsync();
            if(recipe == null) {
                return NotFound("Can't find recipe by id: " + id);
            }
            recipeModel.id = recipe.Id;
            recipeModel.name = recipe.Name;

            // get RecipeFrontImage
            RecipeFrontImage recipeFrontImage = await context.RecipeFrontImage.Where(x => x.Recipe == id).FirstOrDefaultAsync();
            if(recipe == null) {
                return NotFound("Can't find recipe image by recipe id: " + id);
            }
            recipeModel.frontImage = FileHelper.DecodeString(recipeFrontImage.FrontImage);

            // get RecipeIngredientMeasure
            List<IngredientModel> ingredientModels = new List<IngredientModel>();
            var recipeIngredientMeasures = await context.RecipeIngredientMeasure.Where(x => x.Recipe == id).ToListAsync();
            foreach(var recipeIngredientMeasure in recipeIngredientMeasures) {
                Ingredient ingredient = context.Ingredient.Where(x => x.Id == recipeIngredientMeasure.Ingredient).FirstOrDefault();
                Measure measure = context.Measure.Where(x => x.Id == recipeIngredientMeasure.Measure).FirstOrDefault();

                IngredientModel ingredientModel = new IngredientModel();
                ingredientModel.id = Guid.NewGuid().ToString();
                ingredientModel.ingredientName = ingredient.Name;
                ingredientModel.ingredientValue = ingredient.Id.ToString();
                ingredientModel.amount = recipeIngredientMeasure.Amount;
                ingredientModel.measureName = measure.Name;
                ingredientModel.measureValue = measure.Id.ToString();

                ingredientModels.Add(ingredientModel);
            }
            recipeModel.ingredients = ingredientModels;


            // get RecipeSteps
            List<DirectionModel> directionModels = new List<DirectionModel>();
            var steps = await context.RecipeSteps.Where(x => x.Recipe == id).ToListAsync();
            foreach(var step in steps) {
                DirectionModel directionModel = new DirectionModel();
                directionModel.id = Guid.NewGuid().ToString();
                directionModel.sortNumber = step.Sort;
                directionModel.description = step.Step;

                directionModels.Add(directionModel);
            }
            recipeModel.directions = directionModels;

            return Ok(recipeModel);
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
        [HttpGet("ingredient-unique/{name}/{excludedIngredientId}")]
        [ProducesResponseType(typeof(IEnumerable<Boolean>), 200)]
        public async Task<IActionResult> CheckIngredientUniqueness(string name, int excludedIngredientId)
        {
            if (String.IsNullOrWhiteSpace(name)) {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError("Name", "Ingredient name is null or white space.");
                return BadRequest(modelState);
            } else {
                string trimLowercaseName = name.Trim().ToLower();
                var res = await context.Ingredient
                .Where(x => excludedIngredientId < 1 || x.Id != excludedIngredientId)
                .FirstOrDefaultAsync(item => item.Name.ToLower() == trimLowercaseName);
                
                return Ok( (res == null)); // true/false
            }
        }

        // GET api/homecuisine/recipe-exists/{IngredientName}
        [HttpGet("recipe-unique/{name}/{excludedRecipeId}")]
        [ProducesResponseType(typeof(IEnumerable<String>), 200)]
        public async Task<IActionResult> CheckRecipeUniqueness(string name, int excludedRecipeId)
        {
            if (String.IsNullOrWhiteSpace(name)) {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError("Name", "Recipe name is null or white space.");
                return BadRequest(modelState);
            } else {
                string trimLowercaseName = name.Trim().ToLower();
                var res = await context.Recipe
                .Where(x => excludedRecipeId < 1 || x.Id != excludedRecipeId)
                .FirstOrDefaultAsync(item => item.Name.ToLower() == trimLowercaseName);
                return Ok( (res == null)); // true/false

            }
        }

        #endregion

        #region CRUD -> CREATE

        // POST api/homecuisine/ingredient
        [HttpPost]
        [Route("ingredient")]
        public async Task<IActionResult> CreateIngredient([FromBody]Ingredient model)
        {
            model.Name = model.Name.Trim();
            context.Ingredient.Add(model);
            await context.SaveChangesAsync();
            return Ok(model);
        }

        // POST api/homecuisine/recipe
        [HttpPost]
        [Route("recipe")]
        public async Task<IActionResult> CreateRecipe([FromBody]RecipeModel model)
        {
            if( String.IsNullOrWhiteSpace(model.name)) {
                return BadRequest("Recipe name required");
            }
            if( model.name.Length < 3) {
                return BadRequest("Recipe name should have at least 3 characters");
            }
            if( String.IsNullOrWhiteSpace(model.frontImage)) {
                return BadRequest("Front image required");
            }
            // Note: recipe name uniqueness handles by DB

            // create Recipe
            Recipe recipe = new Recipe();
            recipe.Name = model.name;
            context.Recipe.Add(recipe);

            // create RecipeFrontImage
            RecipeFrontImage recipeFrontImage = new RecipeFrontImage();
            recipeFrontImage.FrontImage = FileHelper.EncodeString(model.frontImage);
            context.RecipeFrontImage.Add(recipeFrontImage);
            recipeFrontImage.RecipeNavigation = recipe;

            // create multiple RecipeIngredientMeasure
            foreach(IngredientModel ingredient in model.ingredients) {
                RecipeIngredientMeasure recipeIngredientMeasure = new RecipeIngredientMeasure();
                recipeIngredientMeasure.Ingredient = Int32.Parse(ingredient.ingredientValue);
                recipeIngredientMeasure.Amount = ingredient.amount;
                recipeIngredientMeasure.Measure = Int32.Parse(ingredient.measureValue);
                recipeIngredientMeasure.RecipeNavigation = recipe;
                context.RecipeIngredientMeasure.Add(recipeIngredientMeasure);
            }

            // create multiple RecipeIngredientMeasure
            foreach(DirectionModel direction in model.directions) {
                RecipeSteps recipeSteps = new RecipeSteps();
                recipeSteps.Sort = direction.sortNumber;
                recipeSteps.Step = direction.description.Trim();
                recipeSteps.RecipeNavigation = recipe;
                context.RecipeSteps.Add(recipeSteps);
            }

            await context.SaveChangesAsync();
            return Ok(recipe.Id);
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


        #region CRUD -> UPDATE

        [HttpPut]
        [Route("recipe/{recipeId}")]
        public async Task<IActionResult> UpdateRecipe(int recipeId, [FromBody]RecipeModel model)
        {
            // update Recipe
            Recipe recipe = await context.Recipe.SingleOrDefaultAsync(x => x.Id == recipeId);
            if(recipe == null) {
                return NotFound("Not Found Recipe");
            }
            recipe.Name = model.name;
            context.Recipe.Update(recipe);

            // update RecipeFrontImage
            RecipeFrontImage recipeFrontImage  = await context.RecipeFrontImage.SingleOrDefaultAsync(x => x.Recipe == recipeId);
            if(recipeFrontImage == null) {
                return NotFound("Not Found Recipe Front Image");
            }
            recipeFrontImage.FrontImage = FileHelper.EncodeString(model.frontImage);
            context.RecipeFrontImage.Update(recipeFrontImage);

            // delete old RecipeIngredientMeasures
            var recipeIngredientMeasures = await context.RecipeIngredientMeasure.Where(x => x.Recipe == recipeId).ToListAsync();
            context.RecipeIngredientMeasure.RemoveRange(recipeIngredientMeasures);
            // create multiple RecipeIngredientMeasure
            foreach(IngredientModel ingredient in model.ingredients) {
                RecipeIngredientMeasure recipeIngredientMeasure = new RecipeIngredientMeasure();
                recipeIngredientMeasure.Ingredient = Int32.Parse(ingredient.ingredientValue);
                recipeIngredientMeasure.Amount = ingredient.amount;
                recipeIngredientMeasure.Measure = Int32.Parse(ingredient.measureValue);
                recipeIngredientMeasure.RecipeNavigation = recipe;
                context.RecipeIngredientMeasure.Add(recipeIngredientMeasure);
            }

            // delete old RecipeSteps
            var recipeSteps = await context.RecipeSteps.Where(x => x.Recipe == recipeId).ToListAsync();
            context.RecipeSteps.RemoveRange(recipeSteps);
            // create multiple RecipeSteps
            foreach(DirectionModel direction in model.directions) {
                RecipeSteps recipeStep = new RecipeSteps();
                recipeStep.Sort = direction.sortNumber;
                recipeStep.Step = direction.description.Trim();
                recipeStep.RecipeNavigation = recipe;
                context.RecipeSteps.Add(recipeStep);
            }

            await context.SaveChangesAsync();
            return Ok(recipe.Id);
        }

        #endregion
    }
}
