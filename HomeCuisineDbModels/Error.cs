using System;
using System.Collections.Generic;

namespace DotNetApisForAngularProjects.HomeCuisineDbModels
{
    public partial class Error
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
