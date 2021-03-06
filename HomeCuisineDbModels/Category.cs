﻿using System;
using System.Collections.Generic;

namespace DotNetApisForAngularProjects.HomeCuisineDbModels
{
    public partial class Category
    {
        public Category()
        {
            RecipeCategory = new HashSet<RecipeCategory>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }

        public ICollection<RecipeCategory> RecipeCategory { get; set; }
    }
}
