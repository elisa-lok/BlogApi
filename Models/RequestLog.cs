namespace BlogApi.Models
{
  public class RequestLog
{
    public int Id { get; set; }
    public string Method { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string Query { get; set; } = string.Empty;
    public string RequestBody { get; set; } = string.Empty;
    public string ResponseBody { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public long ProcessingTime { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
}

