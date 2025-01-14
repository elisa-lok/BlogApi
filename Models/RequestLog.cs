using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApi.Models
{
  public class RequestLog
{
    [Column("Id")]
    [Key]
    public int Id { get; set; }

    [Column(name: "Methond", TypeName = "varchar(32)")]
    [Required]
    public string Method { get; set; } = string.Empty;

    [Column(name: "Path", TypeName = "varchar(32)")]
    [Required]
    public string Path { get; set; } = string.Empty;

    [Column(name: "Query")]
    [Required]
    public string Query { get; set; } = string.Empty;

    [Column(name: "RequestBody")]
    [Required]
    public string RequestBody { get; set; } = string.Empty;

    [Column(name: "ResponseBody")]
    [Required]
    public string ResponseBody { get; set; } = string.Empty;

    [Column(name: "StatusCode", TypeName = "varchar(32)")]
    public int StatusCode { get; set; }

    [Column(name: "ProcessingTime")]
    [Required]
    public long ProcessingTime { get; set; }

    [Column(name: "Timestamp", TypeName = "datetimeoffset")]
    [Required]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
}

