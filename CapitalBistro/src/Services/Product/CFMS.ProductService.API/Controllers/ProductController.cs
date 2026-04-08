using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using CFMS.ProductService.API.DTOs;
using CFMS.ProductService.Core.Entities;
using CFMS.ProductService.Core.Interfaces;

namespace CFMS.ProductService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly IDistributedCache _cache;
        private const string CacheKey = "products_list";

        public ProductController(IProductRepository repository, IDistributedCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Try to get from Cache first
            try
            {
                var cachedData = await _cache.GetStringAsync(CacheKey);
                if (!string.IsNullOrEmpty(cachedData))
                {
                    var cachedProducts = JsonSerializer.Deserialize<List<ProductResponse>>(cachedData);
                    return Ok(cachedProducts);
                }
            }
            catch (Exception ex)
            {
                // Fallback to DB if Redis fails
                Console.WriteLine($"Redis Error: {ex.Message}");
            }

            // If User is NOT HQ (means they are Branch Staff), they only see Active products
            bool isHq = User.IsInRole("Admin") || string.IsNullOrEmpty(User.FindFirst("FranchiseId")?.Value);
            
            var data = isHq ? await _repository.GetAllAsync() : await _repository.GetAllActiveAsync();
            var result = data.Select(MapToResponse).ToList();

            // Store in Cache for 1 hour
            try
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                };
                var jsonData = JsonSerializer.Serialize(result);
                await _cache.SetStringAsync(CacheKey, jsonData, options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Redis Error (Set): {ex.Message}");
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) return NotFound();

            bool isHq = User.IsInRole("Admin") || string.IsNullOrEmpty(User.FindFirst("FranchiseId")?.Value);
            if (!isHq && !product.IsActive)
                return Forbid(); // Branch cannot see inactive product details

            return Ok(MapToResponse(product));
        }

        [HttpPost]
        [Authorize(Policy = "HQOnly")]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            var product = new Product
            {
                CategoryId = request.CategoryId,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ImageUrl = request.ImageUrl,
                IsActive = request.IsActive
            };

            if (request.Recipes != null && request.Recipes.Any())
            {
                foreach (var req in request.Recipes)
                {
                    product.ProductRecipes.Add(new ProductRecipe
                    {
                        IngredientId = req.IngredientId,
                        QuantityRequired = req.QuantityRequired
                    });
                }
            }

            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();

            // Invalidate Cache
            await ClearCacheAsync();

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, MapToResponse(product));
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "HQOnly")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateProductRequest request)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) return NotFound();

            product.CategoryId = request.CategoryId;
            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.ImageUrl = request.ImageUrl;
            product.IsActive = request.IsActive;

            // Clear old recipes and assign new ones
            product.ProductRecipes.Clear();
            if (request.Recipes != null)
            {
                foreach (var req in request.Recipes)
                {
                    product.ProductRecipes.Add(new ProductRecipe
                    {
                        ProductId = product.Id,
                        IngredientId = req.IngredientId,
                        QuantityRequired = req.QuantityRequired
                    });
                }
            }

            await _repository.UpdateAsync(product);
            await _repository.SaveChangesAsync();

            // Invalidate Cache
            await ClearCacheAsync();

            return NoContent();
        }

        [HttpPatch("{id}/status")]
        [Authorize(Policy = "HQOnly")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateProductStatusRequest request)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) return NotFound();

            product.IsActive = request.IsActive; // Soft delete / Deactivate
            await _repository.UpdateAsync(product);
            await _repository.SaveChangesAsync();

            // Invalidate Cache
            await ClearCacheAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "HQOnly")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();

            // Invalidate Cache
            await ClearCacheAsync();

            return NoContent();
        }

        private async Task ClearCacheAsync()
        {
            try
            {
                await _cache.RemoveAsync(CacheKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Redis Error (Clear): {ex.Message}");
            }
        }

        private ProductResponse MapToResponse(Product p)
        {
            return new ProductResponse
            {
                Id = p.Id,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name ?? string.Empty,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                IsActive = p.IsActive,
                Recipes = p.ProductRecipes.Select(pr => new RecipeIngredientDto
                {
                    IngredientId = pr.IngredientId,
                    IngredientName = pr.Ingredient?.Name ?? string.Empty,
                    UnitOfMeasure = pr.Ingredient?.UnitOfMeasure ?? string.Empty,
                    QuantityRequired = pr.QuantityRequired
                }).ToList()
            };
        }
    }
}
