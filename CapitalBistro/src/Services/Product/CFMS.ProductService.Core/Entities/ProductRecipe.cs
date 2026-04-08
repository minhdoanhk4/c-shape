using System;

namespace CFMS.ProductService.Core.Entities
{
    public class ProductRecipe
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public Guid IngredientId { get; set; }
        public Ingredient Ingredient { get; set; } = null!;

        public decimal QuantityRequired { get; set; }
    }
}
