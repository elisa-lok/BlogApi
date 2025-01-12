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
          private readonly IPostService _postService;

          public CategoryController(ICategoryService categoryService, IPostService postService)
          {
              _categoryService = categoryService;
              _postService = postService;
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
                var postsInCategory = await _postService.GetPostsByCategoryId(id);
                if(postsInCategory.Any())
                {
                  return BadRequest(new { Message = $"Cannot delete category with associated posts." });
                }
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
                return BadRequest(new { Message = "Invalid request data or ID mismatch." });
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