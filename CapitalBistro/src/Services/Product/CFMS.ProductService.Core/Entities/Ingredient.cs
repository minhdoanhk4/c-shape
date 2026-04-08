using System;
using System.Collections.Generic;

namespace CFMS.ProductService.Core.Entities
{
    public class Ingredient
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string UnitOfMeasure { get; set; } = string.Empty; // e.g. kg, ml, g, piece
        
        public ICollection<ProductRecipe> ProductRecipes { get; set; } = new List<ProductRecipe>();
    }
}
