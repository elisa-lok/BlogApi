using Microsoft.AspNetCore.Mvc;
using BlogApi.Models;
using BlogApi.Services;

namespace BlogApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

      public class CategoryController : ControllerBase
      {
          private readonly ICategoryService _categoryService;

          public CategoryController(ICategoryService categoryService)
          {
            _categoryService = categoryService;
          }

          [HttpGet]
          public async Task<IActionResult> GetCategories()
          {
            var categories = await _categoryService.GetCategoriesAsync();
            return Ok(categories);
          }

          [HttpGet("{id}")]
          public async Task<IActionResult> GetCategory(int id)
          {
            var category = await _categoryService.GetCategoryAsync(id);
            return category == null ? NotFound(new { Message = $"Category with ID {id} not found." }) : Ok(category);
          }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteCategory(int id)
            {
              try
              {
                await _categoryService.DeleteCategoryAsync(id);
                return NoContent();
              }
                catch (KeyNotFoundException)
              {
                return NotFound(new { Message = $"Category with ID {id} not found." });
              }
            }

               [HttpPost]
              public async Task<IActionResult> CreateCategory(Category category)
              {
                if (!ModelState.IsValid)
                {
                  return BadRequest(ModelState);
                }

                var createdCategory = await _categoryService.CreateCategoryAsync(category);
                return CreatedAtAction(nameof(GetCategories), new { id = createdCategory.Id }, createdCategory);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateCategory(int id, Category category)
            {
              if (!ModelState.IsValid || id != category.Id)
              {
                return BadRequest(new { Message = "Invalid request data." });
              }

            try
            {
                var updatedCategory = await _categoryService.UpdateCategoryAsync(id, category);
                return Ok(updatedCategory);
            }
            catch (KeyNotFoundException)
            {
              return NotFound(new { Message = $"Category with ID {id} not found." });
            }
         }
      }
}