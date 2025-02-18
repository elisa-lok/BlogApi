using BlogApi.Models;

namespace BlogApi.Services
{
  public interface ICommentService
  {
    Task<Comment> CreateComment(Comment comment);
    Task<Comment> GetComment(int id);
    Task<IEnumerable<Comment>> GetCommentsByPost(int postId);
    Task DeleteComment(int id);
  }
}
