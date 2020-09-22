using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookListMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookListMVC.Controllers
{
    public class BooksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly ApplicationDBContext _db;
        public BooksController(ApplicationDBContext db)
        {
            _db = db;
        }
        [BindProperty]
        public BookTableMVC Book { get; set; }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            BookTableMVC BookTableMVC = new BookTableMVC();
            if(id==null)
            {
                return View(BookTableMVC);   // Inorder to Insert a new record.
            }
            //update
            BookTableMVC = _db.Book.FirstOrDefault(u => u.id == id);
            if(BookTableMVC == null)
            {
                return NotFound();
            }
            return View(BookTableMVC);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if(ModelState.IsValid)
            {
                if(Book.id==0)
                {
                    _db.Book.Add(Book);
                }
                else
                {
                    _db.Book.Update(Book);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Book);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Book.ToListAsync() });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var bookFromDb = await _db.Book.FirstOrDefaultAsync(u => u.id == id);
            if (bookFromDb == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _db.Book.Remove(bookFromDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Delete Successfuly" });
        }
    }
}