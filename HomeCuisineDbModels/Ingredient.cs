using System;
using System.Collections.Generic;

namespace DotNetApisForAngularProjects.HomeCuisineDbModels
{
    public partial class Ingredient
    {
        public Ingredient()
        {
            RecipeIngredientMeasure = new HashSet<RecipeIngredientMeasure>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }

        public ICollection<RecipeIngredientMeasure> RecipeIngredientMeasure { get; set; }
    }
}
