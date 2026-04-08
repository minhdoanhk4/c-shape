using System;
using System.Collections.Generic;

namespace CFMS.ProductService.API.DTOs
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class CreateCategoryRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class ProductResponse
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        
        public List<RecipeIngredientDto> Recipes { get; set; } = new List<RecipeIngredientDto>();
    }

    public class CreateProductRequest
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        
        public List<CreateRecipeRequest> Recipes { get; set; } = new List<CreateRecipeRequest>();
    }

    public class UpdateProductStatusRequest
    {
        public bool IsActive { get; set; }
    }

    public class IngredientDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string UnitOfMeasure { get; set; } = string.Empty;
    }

    public class CreateIngredientRequest
    {
        public string Name { get; set; } = string.Empty;
        public string UnitOfMeasure { get; set; } = string.Empty;
    }

    public class RecipeIngredientDto
    {
        public Guid IngredientId { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public string UnitOfMeasure { get; set; } = string.Empty;
        public decimal QuantityRequired { get; set; }
    }

    public class CreateRecipeRequest
    {
        public Guid IngredientId { get; set; }
        public decimal QuantityRequired { get; set; }
    }
}
