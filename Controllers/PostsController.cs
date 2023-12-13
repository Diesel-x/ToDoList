using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ToDoList.Data;
using System.ComponentModel.DataAnnotations;
using ToDoList.Areas.Identity.Data;

namespace ToDoList.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _environment;

        public PostsController(
            ApplicationContext context,
            IWebHostEnvironment environment
            )
        {
            _context = context;
            _environment = environment;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
              return _context.Posts != null ? 
                          View(await _context.Posts.ToListAsync()) :
                          Problem("Entity set 'ApplicationContext.Posts'  is null.");
        }

        public async Task<IActionResult> UserPost()
        {
            var user = await _context.Users.SingleAsync(user => user.Email == Request.HttpContext.User.Identity.Name);
            return _context.Posts != null ?
                          View((await _context.Posts
                            .Include(m => m.Author)
                            .Include(m => m.Category)
                            .Include(m => m.Comments)
                            .ThenInclude(c => c.Author)
                            .ToListAsync())
                            .Where(post => post.Author.Id == user.Id)
                            .Reverse()) :
                          Problem("Error");
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(i => i.Category)
                .Include(i => i.Author)
                .Include(i => i.Comments)
                .ThenInclude(c => c.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            var _user = Request.HttpContext.User;
            return View(post);
        }

        // GET: Posts/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = (await _context.Categories.ToListAsync()).Select(item => new SelectListItem { Text = item.Name, Value = item.ID.ToString() });
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Required]IFormFile file, [Required]string Category)
        {
            var post = new Post
            {
                FilePath = file.FileName,
                Author = await _context.Users.SingleAsync(user => user.Email == Request.HttpContext.User.Identity.Name),
                Category = await _context.Categories.FindAsync(int.Parse(Category))
            };
            if (ModelState.IsValid)
            {
                using (var output = new FileStream(Path.Combine(_environment.WebRootPath + "/images/", file.FileName), FileMode.Create))
                {
                    await file.CopyToAsync(output);
                }
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new {id=post.Id});
            }
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FilePath")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'ApplicationContext.Posts'  is null.");
            }
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
          return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
