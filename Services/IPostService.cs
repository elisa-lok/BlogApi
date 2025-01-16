using BlogApi.Models;

namespace BlogApi.Services;

public interface IPostService
{
  Task<Post?> GetPost(int id);
  Task<List<Post>> GetAllPosts();
  Task<List<Post>> GetPostsByCategoryId(int categoryId);
  Task<Post> CreatePost(Post post);
  Task<Post?> UpdatePost(int id, Post post);
  Task DeletePost(int id);
  Task<Post?> PublishPost(int id);
  Task<Post?> UnpublishPost(int id);
  Task<List<Post>> GetPostsByUserId(int userId);
  Task<List<Post>> GetPosts(int pageIndex, int pageSize);
  Task<List<Post>> SearchPosts(string keyword);
}