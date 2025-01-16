using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models
{
  public class Post
  {
    public int Id { get; set; }

    public int UserId { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public PostStatus Status { get; set; } = PostStatus.Published;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
  }
    public enum PostStatus
    {
        unPublished = 0,
        Published = 1,
        Deleted = 2
    }
}