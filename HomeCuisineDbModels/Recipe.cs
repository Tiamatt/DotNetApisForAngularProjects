using System;
using System.Collections.Generic;

namespace DotNetApisForAngularProjects.HomeCuisineDbModels
{
    public partial class Recipe
    {
        public Recipe()
        {
            RecipeCategory = new HashSet<RecipeCategory>();
            RecipeDirection = new HashSet<RecipeDirection>();
            RecipeFrontImage = new HashSet<RecipeFrontImage>();
            RecipeIngredientMeasure = new HashSet<RecipeIngredientMeasure>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }
        public int PreparationTime { get; set; }
        public int Servings { get; set; }

        public ICollection<RecipeCategory> RecipeCategory { get; set; }
        public ICollection<RecipeDirection> RecipeDirection { get; set; }
        public ICollection<RecipeFrontImage> RecipeFrontImage { get; set; }
        public ICollection<RecipeIngredientMeasure> RecipeIngredientMeasure { get; set; }
    }
}
