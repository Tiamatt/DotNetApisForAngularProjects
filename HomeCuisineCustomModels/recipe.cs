using System;
using System.Collections.Generic;
using DotNetApisForAngularProjects.HomeCuisineCustomModels;

namespace DotNetApisForAngularProjects.HomeCuisineCustomModels
{
    public class RecipeModel
    {
        public int? id {get;set;}
        public string name {get; set;}
        public int preparationTime {get;set;}
        public int servings {get;set;}
        public string frontImage {get; set;}
        public List<IngredientModel> ingredients {get; set;}
        public List<DirectionModel> directions {get;set;}
    }
}