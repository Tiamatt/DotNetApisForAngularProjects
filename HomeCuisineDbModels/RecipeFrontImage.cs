using System;
using System.Collections.Generic;

namespace DotNetApisForAngularProjects.HomeCuisineDbModels
{
    public partial class RecipeFrontImage
    {
        public Guid Guid { get; set; }
        public int Recipe { get; set; }
        public byte[] FrontImage { get; set; }

        public Recipe RecipeNavigation { get; set; }
    }
}
