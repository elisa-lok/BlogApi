using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models
{
  public class Category
  {
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<Post> Posts { get; set; } = new List<Post>(); 
  }
}