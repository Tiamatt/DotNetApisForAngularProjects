using System;
using System.Collections.Generic;

namespace DotNetApisForAngularProjects.HomeCuisineDbModels
{
    public partial class RecipeDirection
    {
        public Guid Guid { get; set; }
        public int Recipe { get; set; }
        public string Direction { get; set; }
        public int Sort { get; set; }

        public Recipe RecipeNavigation { get; set; }
    }
}
