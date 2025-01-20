using System.Diagnostics;
using BlogApi.Data;
using BlogApi.Models;

namespace BlogApi.Middlewares
{
  public class LoggingMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, BlogContext blogContext)
    {
      var request = await FormatRequest(context.Request);

      var originalBodyStream = context.Response.Body;
      using var ResponseBody = new MemoryStream();
      context.Response.Body = ResponseBody;

      var stopwatch = Stopwatch.StartNew();

      try
      {
        await _next(context);
      }
      finally
      {
        stopwatch.Stop();

        var response = await FormatResponse(context.Response);
        var log = new RequestLog
        {
          Method = context.Request.Method,
          Path = context.Request.Path,
          Query = context.Request.QueryString.ToString(),
          RequestBody = request,
          ResponseBody = response,
          StatusCode = context.Response.StatusCode,
          ProcessingTime = stopwatch.ElapsedMilliseconds
        };

        blogContext.RequestLogs.Add(log);
        await blogContext.SaveChangesAsync();

        await ResponseBody.CopyToAsync(originalBodyStream);
      }
    }

      private async Task<string> FormatRequest(HttpRequest request)
    {
        request.EnableBuffering();

        var body = string.Empty;
        if (request.ContentLength > 0)
        {
            using var reader = new StreamReader(request.Body, leaveOpen: true);
            body = await reader.ReadToEndAsync();
            request.Body.Position = 0; 
        }

        return body;
    }

    private async Task<string> FormatResponse(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var text = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);

        return text;
    }
  } 
}