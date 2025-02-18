using BlogApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApi.Services
{
  public class CommentService : ICommentService
  {
    private readonly List<Comment> _comments = new();

    public async Task<Comment> CreateComment(Comment comment)
    {
      comment.Id = _comments.Count + 1;

      if (comment.ParentCommentId.HasValue)
      {
        var parentComment = _comments.FirstOrDefault(c => c.Id == comment.ParentCommentId);
        if (parentComment != null)
        {
          parentComment.Replies.Add(comment);
        }
      }

      _comments.Add(comment);
      return await Task.FromResult(comment);
    }

    public async Task<Comment> GetComment(int id)
    {
      return await Task.FromResult(_comments.FirstOrDefault(c => c.Id == id));
    }

    public async Task<IEnumerable<Comment>> GetCommentsByPost(int postId)
    {
      return await Task.FromResult(_comments.Where(c => c.PostId == postId && c.ParentCommentId == null));
    }

    public async Task DeleteComment(int id)
    {
      var comment = _comments.FirstOrDefault(c => c.Id == id);
      if (comment != null)
      {
        DeleteChildComments(comment);
        _comments.Remove(comment);
      }
      await Task.CompletedTask;
    }

    private void DeleteChildComments(Comment comment)
    {
      foreach (var reply in comment.Replies.ToList())
      {
        DeleteChildComments(reply);
        _comments.Remove(reply);
      }
    }
  }
}
