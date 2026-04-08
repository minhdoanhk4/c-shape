using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CFMS.ProductService.API.DTOs;
using CFMS.ProductService.Core.Entities;
using CFMS.ProductService.Core.Interfaces;

namespace CFMS.ProductService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientRepository _repository;

        public IngredientController(IIngredientRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _repository.GetAllAsync();
            var result = data.Select(i => new IngredientDto
            {
                Id = i.Id,
                Name = i.Name,
                UnitOfMeasure = i.UnitOfMeasure
            });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var ingredient = await _repository.GetByIdAsync(id);
            if (ingredient == null) return NotFound();

            return Ok(new IngredientDto
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                UnitOfMeasure = ingredient.UnitOfMeasure
            });
        }

        [HttpPost]
        [Authorize(Policy = "HQOnly")]
        public async Task<IActionResult> Create([FromBody] CreateIngredientRequest request)
        {
            var ingredient = new Ingredient
            {
                Name = request.Name,
                UnitOfMeasure = request.UnitOfMeasure
            };
            
            await _repository.AddAsync(ingredient);
            await _repository.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetById), new { id = ingredient.Id }, ingredient);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "HQOnly")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateIngredientRequest request)
        {
            var ingredient = await _repository.GetByIdAsync(id);
            if (ingredient == null) return NotFound();

            ingredient.Name = request.Name;
            ingredient.UnitOfMeasure = request.UnitOfMeasure;

            await _repository.UpdateAsync(ingredient);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "HQOnly")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();
            return NoContent();
        }
    }
}
