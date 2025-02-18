using Microsoft.AspNetCore.Mvc;
using BlogApi.Models;
using BlogApi.Services;

namespace BlogApi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class CommentController : ControllerBase
  {
    private readonly ICommentService _commentService;
    private readonly IPostService _postService;

    public CommentController(ICommentService commentService, IPostService postService)
    {
      _commentService = commentService;
      _postService = postService;
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Comment>> GetComment(int id)
    {
      var comment = await _commentService.GetComment(id);
      if (comment == null)
      {
        return NotFound();
      }

      return Ok(comment);
    }

    [HttpGet("post/{postId}")]
    public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsByPost(int postId)
    {
      var comments = await _commentService.GetCommentsByPost(postId);
      return Ok(comments);
    }


    [HttpPost]
    public async Task<ActionResult<Comment>> CreateComment([FromBody] Comment comment)
    {
      if (comment == null)
      {
        return BadRequest("Invalid comment data.");
      }

      var post = await _postService.GetPost(comment.PostId);
      if (post == null)
      {
        return BadRequest("Post not found.");
      }

      var createdComment = await _commentService.CreateComment(comment);
      return CreatedAtAction(nameof(GetComment), new { id = createdComment.Id }, createdComment);
    }
   
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
      var existingComment = await _commentService.GetComment(id);
      if (existingComment == null)
      {
        return NotFound();
      }

      await _commentService.DeleteComment(id);
      return NoContent();
    }
  }
}
