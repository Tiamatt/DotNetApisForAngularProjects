using System;
using System.Collections.Generic;

namespace DotNetApisForAngularProjects.HomeCuisineDbModels
{
    public partial class RecipeIngredientMeasure
    {
        public Guid Guid { get; set; }
        public int Recipe { get; set; }
        public int Ingredient { get; set; }
        public string Amount { get; set; }
        public int Measure { get; set; }

        public Ingredient IngredientNavigation { get; set; }
        public Measure MeasureNavigation { get; set; }
        public Recipe RecipeNavigation { get; set; }
    }
}
