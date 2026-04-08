using System;
using System.Collections.Generic;

namespace CFMS.ProductService.Core.Entities
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true; // For logical deletion/hide
        
        public ICollection<ProductRecipe> ProductRecipes { get; set; } = new List<ProductRecipe>();
    }
}
