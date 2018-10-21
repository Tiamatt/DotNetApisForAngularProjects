using System;
using System.Collections.Generic;
using DotNetApisForAngularProjects.HomeCuisineCustomModels;

namespace DotNetApisForAngularProjects.HomeCuisineCustomModels
{
    public class RecipeModel
    {
        public int? id {get;set;}
        public string name {get; set;}
        public string frontImage {get; set;}
        public List<IngredientModel> ingredients {get; set;}
        public List<DirectionModel> directions {get;set;}
    }
}