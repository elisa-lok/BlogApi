using BlogApi.Models;

namespace BlogApi.Services;

public class PostService() {
  private static readonly List<Post> AllPosts = new();

  public Task CreatePost(Post post) {
    AllPosts.Add(post);
    return Task.CompletedTask;
  }

  public Task DeletePost(int id) {
    var post = AllPosts.FirstOrDefault(p => p.Id == id);
    if (post != null) {
      AllPosts.Remove(post);
    }
    return Task.CompletedTask;
  }

  public Task<List<Post>> GetAllPosts() {
    return Task.FromResult(AllPosts);
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
    }
    return Task.FromResult(existingPost);
  }
}