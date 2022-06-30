using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bookstore.Data;
using Bookstore.Models;
using Bookstore.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Bookstore.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookstoreContext _context;

        public BooksController(BookstoreContext context)
        {
            _context = context;
        }

        // GET: Books
        //[Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Index(string bookGenre, string? bookAuthor ,string searchString)
        {
            IQueryable<Book> books = _context.Book.AsQueryable();
            IQueryable<string> genreQuery = _context.Book.OrderBy(m => m.Genre).Select(m => m.Genre).Distinct();
            IQueryable<string> authorQuery = _context.Author.OrderBy(m => m.Id).Select(m => m.FullName).Distinct();
          
            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Title.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(bookGenre))
            {
                books = books.Where(x => x.Genre == bookGenre);
            }
            if (!string.IsNullOrEmpty(bookAuthor))
            {
                string[] names = bookAuthor.Split(" ");
                var author = _context.Author.Where(x => x.FirstName == names[0] && x.LastName == names[1]).First();
                books = books.Where(x => x.Author.Id == author.Id);
            }
            books = books.Include(m => m.Users).ThenInclude(m => m.User);
            var vm = new BooksFilterVM
            {
                Genres = new SelectList(await genreQuery.ToListAsync()),
                Authors = new SelectList(await authorQuery.ToListAsync()),
                books = await books.ToListAsync()
            };
            return View(vm);
        }

        // GET: Books/Details/5
        //[Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "FullName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Title,PublicationDate,Genre,Pages,Price,Description,Picture,AuthorId")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "FullName", book.AuthorId);
            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            BookEditPicture vm = new BookEditPicture
            {
                book = book,
                pictureName = book.Picture
            };
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "FullName", book.AuthorId);
            return View(vm);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, BookEditPicture vm)
        {
            if (id != vm.book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (vm.pictureFile != null)
                    {
                        string uniqueFileName = UploadedFile(vm);
                        vm.book.Picture = uniqueFileName;
                    }
                    else
                    {
                        vm.book.Picture = vm.pictureName;
                    }
                    _context.Update(vm.book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(vm.book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { id = vm.book.Id }) ;
            }
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "FullName", vm.book.AuthorId);
            return View(vm);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Book == null)
            {
                return Problem("Entity set 'BookstoreContext.Book'  is null.");
            }
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        private bool BookExists(int id)
        {
          return (_context.Book?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [Authorize(Roles = "Admin")]
        private string UploadedFile(BookEditPicture vm)
        {
            string uniqueFileName = null;

            if (vm.pictureFile != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/pictures");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(vm.pictureFile.FileName);
                string fileNameWithPath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    vm.pictureFile.CopyTo(stream);
                }
            }
            return uniqueFileName;
        }
       // [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> AuthorBooks(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var author = await _context.Author.FirstOrDefaultAsync(x => x.Id == id);
            ViewBag.Author = author.FullName;
            IQueryable<Book> books = _context.Book.Where(x => x.AuthorId == id);
            await _context.SaveChangesAsync();
            return View(await books.ToListAsync());
        }
    }
}
