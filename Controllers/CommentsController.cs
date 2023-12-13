using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _environment;

        public CommentsController(
            ApplicationContext context,
            IWebHostEnvironment environment
            )
        {
            _context = context;
            _environment = environment;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(int relatedPostId, string Text)
        {
            if (string.IsNullOrWhiteSpace(Text)) return BadRequest("Text empty");

            var relatedPost = _context.Posts.Find(relatedPostId);
            var user = await _context.Users.SingleAsync(user => user.Email == Request.HttpContext.User.Identity.Name);
            if (user == null) return BadRequest();
            var comment = new Comment { Author = user, Text = Text };
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            relatedPost.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
