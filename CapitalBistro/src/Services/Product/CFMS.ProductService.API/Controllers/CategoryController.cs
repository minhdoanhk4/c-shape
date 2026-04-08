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
    [Authorize] // Require basic login to access anything
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _repository;
        private readonly IDistributedCache _cache;
        private const string CacheKey = "categories_list";

        public CategoryController(ICategoryRepository repository, IDistributedCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        // Available to everyone (HQ + Branch)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Try to get from Cache first
            try
            {
                var cachedData = await _cache.GetStringAsync(CacheKey);
                if (!string.IsNullOrEmpty(cachedData))
                {
                    var cachedCategories = JsonSerializer.Deserialize<List<CategoryDto>>(cachedData);
                    return Ok(cachedCategories);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Redis Error: {ex.Message}");
            }

            var data = await _repository.GetAllAsync();
            var result = data.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList();

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
            var category = await _repository.GetByIdAsync(id);
            if (category == null) return NotFound();

            return Ok(new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            });
        }

        // HQ Only
        [HttpPost]
        [Authorize(Policy = "HQOnly")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
        {
            var category = new Category
            {
                Name = request.Name,
                Description = request.Description
            };
            
            await _repository.AddAsync(category);
            await _repository.SaveChangesAsync();

            // Invalidate Cache
            await ClearCacheAsync();
            
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "HQOnly")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateCategoryRequest request)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null) return NotFound();

            category.Name = request.Name;
            category.Description = request.Description;

            await _repository.UpdateAsync(category);
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
    }
}
