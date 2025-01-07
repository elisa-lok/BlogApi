using BlogApi.Models;

namespace BlogApi.Services;

public interface IPostService
{
  Task<Post?> GetPost(int id);
  Task<List<Post>> GetAllPosts();
  Task<Post> CreatePost(Post post);
  Task<Post?> UpdatePost(int id, Post post);
  Task DeletePost(int id); 
}