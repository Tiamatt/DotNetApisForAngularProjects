using System;
using System.Collections.Generic;

namespace DotNetApisForAngularProjects.HomeCuisineDbModels
{
    public partial class Recipe
    {
        public Recipe()
        {
            RecipeFrontImage = new HashSet<RecipeFrontImage>();
            RecipeIngredientMeasure = new HashSet<RecipeIngredientMeasure>();
            RecipeSteps = new HashSet<RecipeSteps>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }

        public ICollection<RecipeFrontImage> RecipeFrontImage { get; set; }
        public ICollection<RecipeIngredientMeasure> RecipeIngredientMeasure { get; set; }
        public ICollection<RecipeSteps> RecipeSteps { get; set; }
    }
}
