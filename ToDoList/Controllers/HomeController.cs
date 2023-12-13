using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ApplicationContext _context;

		public HomeController(ILogger<HomeController> logger, ApplicationContext context)
		{
			_logger = logger;
			_context = context;
		}

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var _posts = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Author)
                .Include(p => p.Category)
                .ToListAsync();
            _posts.Reverse();
            var _user = Request.HttpContext.User;
            return View("Index",_posts.Take(5));
        }

        [AllowAnonymous]
        public async Task<IActionResult> IndexAll(string Sort="0")
        {
            var _posts = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Author)
                .ToListAsync();
            _posts.Reverse();
            if (Sort != "0" && Sort != "1")
                _posts = _posts.Where(post => post.Category.ID == int.Parse(Sort)).ToList();
            var _user = Request.HttpContext.User;
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View("IndexAll", _posts);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}