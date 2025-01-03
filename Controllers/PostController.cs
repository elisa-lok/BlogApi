using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogApi.Data;
using BlogApi.Models;

namespace BlogApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly BlogContext _context;

        public PostController(BlogContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _context.Posts.OrderByDescending(p => p.CreatedAt).ToListAsync();
            return Ok(posts);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] Post post)
        {
            post.CreatedAt = DateTime.UtcNow;
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPosts), new { id = post.Id }, post);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] Post post)
        {
            if (id != post.Id)
            {
                return BadRequest();
            }

            _context.Entry(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
