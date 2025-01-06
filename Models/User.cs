namespace BlogApi.Models;

public class User 
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;

  public string Email { get; set; } = string.Empty;

  public string Password { get; set; } = string.Empty;

  public string Role { get; set; } = string.Empty;

  public string PhoneNumber { get; set; } = string.Empty;

  public string Country { get; set; } = string.Empty; 

  public DateTime CreatedAt { get; set; } = DateTime.Now;

  public DateTime UpdatedAt { get; set; } = DateTime.Now;

  public string Status { get; set; } = string.Empty;
}