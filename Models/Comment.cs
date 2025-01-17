namespace BlogApi.Models
{
  public class Comment
{
    public int Id { get; set; } 

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 

    public int PostId { get; set; }
    public Post Post { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int? ParentCommentId { get; set; }
    public Comment ParentComment { get; set; } = null!;

    public List<Comment> Replies { get; set; } = new();
  }
}
