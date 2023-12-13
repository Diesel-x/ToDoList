using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public AdminController(ApplicationContext context)
        {
            _context = context;
        }
        [HttpPost]
        [Route("DeleteComment")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            try
            {
                var comment = await _context.Comments.SingleAsync(c => c.Id == commentId);
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch
            {
                return Problem();
            }
        }

        [HttpPost]
        [Route("DeletePost")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            try
            {
                var post = await _context.Posts.Include(i => i.Comments).SingleAsync(c => c.Id == postId);
                if (post.Comments.Any())
                {
                    foreach (var comment in post.Comments)
                        _context.Comments.Remove(comment);
                    await _context.SaveChangesAsync();
                }
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch
            {
                return Problem();
            }
        }
    }
}
