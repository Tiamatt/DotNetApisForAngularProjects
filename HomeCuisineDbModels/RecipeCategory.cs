using System;
using System.Collections.Generic;

namespace DotNetApisForAngularProjects.HomeCuisineDbModels
{
    public partial class RecipeCategory
    {
        public Guid Guid { get; set; }
        public int Recipe { get; set; }
        public int Category { get; set; }

        public Category CategoryNavigation { get; set; }
        public Recipe RecipeNavigation { get; set; }
    }
}
