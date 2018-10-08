using System;
using System.Collections.Generic;

namespace DotNetApisForAngularProjects.HomeCuisineDbModels
{
    public partial class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }
    }
}
