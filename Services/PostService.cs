using BlogApi.Models;

namespace BlogApi.Services;

public class PostService: IPostService
{
  private static readonly List<Post> AllPosts = new()
  {
    new Post { Id = 1, UserId = 1,Title = "Introduction to C#", Content = "This is a post about C# programming basics.", CategoryId = 1 },
    new Post { Id = 2, UserId = 1,Title = "10 Tips for Healthy Living", Content = "A post about maintaining a healthy lifestyle.", CategoryId = 3 },
    new Post { Id = 3, UserId = 1,Title = "Tech Trends in 2025", Content = "This post discusses the latest technology trends in 2025.", CategoryId = 1 },
    new Post { Id = 4, UserId = 1,Title = "The Importance of Mental Health", Content = "This post explores the importance of mental health.", CategoryId = 3 },
    new Post { Id = 5, UserId = 1,Title = "How to Build a Personal Website", Content = "A step-by-step guide on building a personal website.", CategoryId = 1 },
    new Post { Id = 6, UserId = 1,Title = "Best Lifestyle Habits for Success", Content = "Post about successful lifestyle habits for personal growth.", CategoryId = 2 },
    new Post { Id = 7, UserId = 1,Title = "Health Benefits of Meditation", Content = "Exploring the health benefits of regular meditation practice.", CategoryId = 3 }
  };

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

  public Task<Post?> PublishPost(int id)
  {
    var post = AllPosts.FirstOrDefault(x => x.Id == id);
    if (post != null)
    {
      post.Status = PostStatus.Published;
    }

      return Task.FromResult(post);
   }

  public Task<Post?> UnpublishPost(int id)
  {
    var post = AllPosts.FirstOrDefault(x => x.Id == id);
    if (post != null)
    {
      post.Status = PostStatus.unPublished;
    }

    return Task.FromResult(post);
  }

  public Task<List<Post>> GetPostsByUserId(int userId)
  {
    return Task.FromResult(AllPosts.Where(x => x.UserId == userId).ToList());
  }

  public Task<List<Post>> GetPosts(int pageIndex, int pageSize)
  {
    return Task.FromResult(AllPosts.Skip(pageIndex * pageSize).Take(pageSize).ToList());
  }

  public Task<List<Post>> SearchPosts(string keyword)
  {
     return Task.FromResult(AllPosts.Where(x => x.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList());
  }
}