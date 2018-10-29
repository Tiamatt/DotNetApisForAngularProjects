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


        // GET api/homecuisine
        [HttpGet] // for testing
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "HomeCuisine1", "HomeCuisine2", "HomeCuisine3" };
        }

        #region CRUD -> READ -> GET SINGLE ITEM BY id

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
            recipeModel.preparationTime = recipe.PreparationTime;
            recipeModel.servings = recipe.Servings;

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
                ingredientModel.flag = false;

                ingredientModels.Add(ingredientModel);
            }
            recipeModel.ingredients = ingredientModels;


            // get RecipeDirection
            List<DirectionModel> directionModels = new List<DirectionModel>();
            var recipeDirections = await context.RecipeDirection.Where(x => x.Recipe == id).ToListAsync();
            foreach(var recipeDirection in recipeDirections) {
                DirectionModel directionModel = new DirectionModel();
                directionModel.id = Guid.NewGuid().ToString();
                directionModel.sortNumber = recipeDirection.Sort;
                directionModel.description = recipeDirection.Direction;
                directionModels.Add(directionModel);
            }
            recipeModel.directions = directionModels;

            // get RecipeCategory
            List<FilterModel> categoryModels = new List<FilterModel>();
            var recipeCategories = await context.RecipeCategory.Where(x => x.Recipe == id).ToListAsync();
            var categories = await context.Category.ToListAsync();
            foreach(var recipeCategory in recipeCategories) {
                FilterModel categoryModel = new FilterModel();
                categoryModel.value = recipeCategory.Category.ToString();
                categoryModel.name = categories.Where(x => x.Id == recipeCategory.Category).Select(x => x.Name).FirstOrDefault();
                categoryModels.Add(categoryModel);
            }
            recipeModel.categories = categoryModels;
 
            return Ok(recipeModel);
        }

        #endregion

        #region CRUD -> READ -> GET LIST (returns list of items)

        // GET api/homecuisine/filter/{entityName}
        [HttpGet("filters/{entityName}")]
        [ProducesResponseType(typeof(IEnumerable<FilterModel>), 200)]
        public async Task<IActionResult> GetFilters(string entityName)
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
                case "category":
                  res = await context.Category
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


        [HttpGet("recipes/{categoryId}")]
        // [ProducesResponseType(typeof(IEnumerable<RecipeShortModel>), 200)]
        public async Task<IActionResult> GetRecipesByCategory(int categoryId){
            List<RecipeShortModel> recipeShortModels = new List<RecipeShortModel>();
            
            List<int> recipeIds = await context.RecipeCategory
            .Where(a => a.Category == categoryId)
            .Select(b => b.Recipe)
            .ToListAsync();

            recipeShortModels = await context.Recipe
            .Join(context.RecipeFrontImage, r => r.Id, rfi => rfi.Recipe, (r, rfi) => new RecipeShortModel{
                id = r.Id,
                name = r.Name,
                preparationTime = r.PreparationTime,
                servings = r.Servings,
                countIngredients = context.RecipeIngredientMeasure.Where(i => i.Recipe == r.Id).Count(),
                frontImage = FileHelper.DecodeString(rfi.FrontImage), 
            })
            .Where(a => recipeIds.IndexOf(a.id) > -1)
            .ToListAsync();

            return Ok(recipeShortModels);
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


        [HttpGet("category-unique/{name}/{excludedCategoryId}")]
        [ProducesResponseType(typeof(IEnumerable<Boolean>), 200)]
        public async Task<IActionResult> CheckCategoryUniqueness(string name, int excludedCategoryId)
        {
            if (String.IsNullOrWhiteSpace(name)) {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError("Name", "Category name is null or white space.");
                return BadRequest(modelState);
            } else {
                string trimLowercaseName = name.Trim().ToLower();
                var res = await context.Category
                .Where(x => excludedCategoryId < 1 || x.Id != excludedCategoryId)
                .FirstOrDefaultAsync(item => item.Name.ToLower() == trimLowercaseName);
                
                return Ok( (res == null)); // true/false
            }
        }


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

        // POST api/homecuisine/category
        [HttpPost]
        [Route("category")]
        public async Task<IActionResult> CreateCategory([FromBody]FilterModel model)
        {
            Category newCategory = new Category();
            newCategory.Name = model.name.Trim();
            context.Category.Add(newCategory);
            await context.SaveChangesAsync();
            return Ok(newCategory);
        }

        // POST api/homecuisine/recipe
        [HttpPost]
        [Route("recipe")]
        public async Task<IActionResult> CreateRecipe([FromBody]RecipeModel model)
        {
            string recipeError = ValidateRecipe(model);
            if ( !String.IsNullOrEmpty(recipeError)) {
                return BadRequest(recipeError);
            }

            // create Recipe
            Recipe recipe = new Recipe();
            recipe.Name = model.name;
            recipe.PreparationTime = model.preparationTime;
            recipe.Servings = model.servings;
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
                RecipeDirection recipeDirection = new RecipeDirection();
                recipeDirection.Sort = direction.sortNumber;
                recipeDirection.Direction = direction.description.Trim();
                recipeDirection.RecipeNavigation = recipe;
                context.RecipeDirection.Add(recipeDirection);
            }

            // create multiple RecipeCategory
            foreach(FilterModel category in model.categories) {
                RecipeCategory recipeCategory = new RecipeCategory();
                recipeCategory.Category = Int32.Parse(category.value);
                recipeCategory.RecipeNavigation = recipe;
                context.RecipeCategory.Add(recipeCategory);
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
            string recipeError = ValidateRecipe(model);
            if ( !String.IsNullOrEmpty(recipeError)) {
                return BadRequest(recipeError);
            }

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

            // delete old RecipeDirections
            var recipeDirections = await context.RecipeDirection.Where(x => x.Recipe == recipeId).ToListAsync();
            context.RecipeDirection.RemoveRange(recipeDirections);
            // create multiple RecipeDirections
            foreach(DirectionModel direction in model.directions) {
                RecipeDirection recipeDirection = new RecipeDirection();
                recipeDirection.Sort = direction.sortNumber;
                recipeDirection.Direction = direction.description.Trim();
                recipeDirection.RecipeNavigation = recipe;
                context.RecipeDirection.Add(recipeDirection);
            }

            // delete old RecipeCategory
            var recipeCategories = await context.RecipeCategory.Where(x => x.Recipe == recipeId).ToListAsync();
            context.RecipeCategory.RemoveRange(recipeCategories);
            // create multiple RecipeCategory
            foreach(FilterModel category in model.categories) {
                RecipeCategory recipeCategory = new RecipeCategory();
                recipeCategory.Category = Int32.Parse(category.value);
                recipeCategory.RecipeNavigation = recipe;
                context.RecipeCategory.Add(recipeCategory);
            }

            await context.SaveChangesAsync();
            return Ok(recipe.Id);
        }

        #endregion
    
        #region private RE-USABLE methods 
        
        private string ValidateRecipe(RecipeModel model) {

            // Note: recipe name uniqueness handles by DB

            if( String.IsNullOrWhiteSpace(model.name)) {
                return "Recipe name required";
            }
            if( model.name.Length < 3) {
                return"Recipe name should have at least 3 characters";
            }
            if( String.IsNullOrWhiteSpace(model.frontImage)) {
                return "Front image required";
            }
            if(model.preparationTime < 1) {
                return"Preparation time can't be less than 1";
            }
            if(model.servings < 1) {
                return"Servings can't be less than 1 ";
            }

            return null; // no error
        }

        #endregion
    }
}
