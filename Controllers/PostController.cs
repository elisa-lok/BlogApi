using Microsoft.AspNetCore.Mvc;
using BlogApi.Models;
using BlogApi.Services;

namespace BlogApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ICategoryService _categoryService;

        public PostController(IPostService postService, ICategoryService categoryService)
        {
            _postService = postService;
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost(Post post)
        {
            if(post == null)
            {
                return BadRequest("Post data is required");
            }

            var category = await _categoryService.GetCategoryAsync(post.CategoryId);
            if(category == null)
            {
              return BadRequest("Invalid CategoryId");
            }

            post.Category = category;
            var createdPost = await _postService.CreatePost(post);
            return CreatedAtAction(nameof(GetPost), new { id = createdPost.Id }, createdPost);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePost(int id)
        {
            await _postService.DeletePost(id);
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<Post>> GetAllPosts()
        {
            var posts = await _postService.GetAllPosts();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _postService.GetPost(id);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePost(int id, Post post)
        {
            if (id != post.Id)
            {
                return BadRequest("Post ID mismatch");
            }

            var category = await _categoryService.GetCategoryAsync(post.CategoryId);
            if(category == null)
            {
                return BadRequest("Invalid CategoryId");
            }

            post.Category = category;

            var UpdatePost = await _postService.UpdatePost(id, post);
            if(UpdatePost == null){
                return NotFound();
            }

            return Ok(UpdatePost);
        }

   
    }
}
