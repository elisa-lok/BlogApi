using BlogApi.Models;

namespace BlogApi.Services;

public class PostService: IPostService
{
  private static readonly List<Post> AllPosts = new();

  public Task<Post> CreatePost(Post post) {
    post.Id = AllPosts.Any() ? AllPosts.Max(p => p.Id) + 1 : 1;
    AllPosts.Add(post);
    return Task.FromResult(post);
  }

  public Task DeletePost(int id) {
    var post = AllPosts.FirstOrDefault(p => p.Id == id);
    if (post == null) {
      throw new KeyNotFoundException();
    }
    AllPosts.Remove(post);
    return Task.CompletedTask;
  }

  public Task<List<Post>> GetAllPosts() {
    return Task.FromResult(AllPosts);
  }

  public Task<List<Post>> GetPostsByCategoryId(int categoryId)
  {
    var posts = AllPosts.Where(p => p.CategoryId == categoryId).ToList();
    return Task.FromResult(posts);
  }

  public Task<Post?> GetPost(int id){
    var post = AllPosts.FirstOrDefault(p => p.Id == id);
    return Task.FromResult(post);
  }

  public Task<Post?> UpdatePost(int id, Post post) {
    var existingPost = AllPosts.FirstOrDefault(p => p.Id == id);
    if (existingPost != null) {
      existingPost.Title = post.Title;
      existingPost.Content = post.Content;
      existingPost.CategoryId = post.CategoryId;
    }
    return Task.FromResult(existingPost);
  }
}