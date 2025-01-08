using BlogApi.Models;

namespace BlogApi.Services;

public class CategoryService: ICategoryService
{
     // Use a static list as a data store for simplicity.
    private static readonly List<Category> Categories = new()
    {
        new() { Id = 1, Name = "Technology", Description = "Posts related to technology." },
        new() { Id = 2, Name = "Lifestyle", Description = "Posts about lifestyle and daily activities." },
        new() { Id = 3, Name = "Health", Description = "Posts related to health and wellness." }
    };
    public Task<List<Category>> GetCategoriesAsync()
    {
        return Task.FromResult(Categories);
    }

    public Task<Category?> GetCategoryAsync(int id){
        var category = Categories.FirstOrDefault(c => c.Id == id);
        return Task.FromResult(category);
    }

    public Task<Category> CreateCategoryAsync(Category category)
    {
        category.Id = Categories.Count + 1;
        Categories.Add(category);
        return Task.FromResult(category);
    }

    public Task<Category> UpdateCategoryAsync(int id, Category category)
    {
         var index = Categories.FindIndex(p => p.Id == id);
         if(index == -1){
            throw new KeyNotFoundException();
          }
        Categories[index] = category;
        return Task.FromResult(category);
    }

    public Task DeleteCategoryAsync(int id)
    {
        var category = Categories.FirstOrDefault(c => c.Id == id);
        if(category == null)
        {
            throw new InvalidOperationException($"Category with id {id} not found.");
        }
        Categories.Remove(category);
        return Task.CompletedTask;
    }
}