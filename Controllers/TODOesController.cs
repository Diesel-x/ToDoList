using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ToDoList.Areas.Identity.Data;
using ToDoList.Data;

namespace Zetix.Controllers
{
    public class TODOesController : Controller
    {
        private readonly ApplicationContext _context;

        public TODOesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: TODOes
        public async Task<IActionResult> Index()
        {
            var userEmail = Request.HttpContext.User.Identity.Name;
            var user = await _context.Users.SingleOrDefaultAsync(user => user.Email == userEmail);

            if (user != null)
            {
                var userTodos = _context.TODOes.Where(post => post.Author.Id == user.Id).ToList();

                return userTodos.Any() ?
                           View(userTodos) :
                           Problem("Набор сущностей 'ApplicationContext.TODO' пуст для данного пользователя.");
            }
            else
            {
                // Обработка ситуации, когда пользователь не найден по указанной электронной почте.
                return View();
            }
        }

        // GET: TODOes/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.TODOes == null)
            {
                return NotFound();
            }

            var tODO = await _context.TODOes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tODO == null)
            {
                return NotFound();
            }

            return View(tODO);
        }

        // GET: TODOes/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: TODOes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Term,Priority,IsDone")] TODO tODO)
        {
            var TODO = new TODO
            {
                Title = tODO.Title,
                Description = tODO.Description,
                Author = await _context.Users.SingleAsync(user => user.Email == User.Identity.Name),
                IsDone = tODO.IsDone,
                Term = tODO.Term,
                Priority = tODO.Priority
            };
            Console.WriteLine(tODO.Term);
            //if (ModelState.IsValid)
            //{
                _context.Add(TODO);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}

            return View(TODO);
        }

        // GET: TODOes/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.TODOes == null)
            {
                return NotFound();
            }

            var tODO = await _context.TODOes.FindAsync(id);
            if (tODO == null)
            {
                return NotFound();
            }
            return View(tODO);
        }

        // POST: TODOes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Title,Description,Priority,IsDone")] TODO tODO)
        {
            if (id != tODO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tODO);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TODOExists(tODO.Id))
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
            return View(tODO);
        }

        // GET: TODOes/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.TODOes == null)
            {
                return NotFound();
            }

            var tODO = await _context.TODOes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tODO == null)
            {
                return NotFound();
            }

            return View(tODO);
        }

        // POST: TODOes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.TODOes == null)
            {
                return Problem("Entity set 'ApplicationContext.TODO'  is null.");
            }
            var tODO = await _context.TODOes.FindAsync(id);
            if (tODO != null)
            {
                _context.TODOes.Remove(tODO);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TODOExists(long id)
        {
          return (_context.TODOes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
