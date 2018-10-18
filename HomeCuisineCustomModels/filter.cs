namespace DotNetApisForAngularProjects.HomeCuisineCustomModels
{
    public class FilterModel
    {
        public string name {get; set;}
        public string value {get;set;}
        public bool? selected {get; set;} //  'checked' is a reserved word in C#
    }
}