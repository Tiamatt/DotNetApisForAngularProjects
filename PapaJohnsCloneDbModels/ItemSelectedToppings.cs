using System;
using System.Collections.Generic;

namespace DotNetApisForAngularProjects.PapaJohnsCloneDbModels
{
    public partial class ItemSelectedToppings
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int ToppingId { get; set; }

        public Item Item { get; set; }
        public Topping Topping { get; set; }
    }
}
