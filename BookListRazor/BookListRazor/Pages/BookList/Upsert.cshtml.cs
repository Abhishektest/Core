using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookListRazor.Pages.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookListRazor.Pages.BookList
{
    public class UpsertModel : PageModel
    {
        private readonly ApplicationDBContext _db;

        public UpsertModel(ApplicationDBContext db)
        {
            _db = db;
        }
        [BindProperty]
        public Book Book { get; set; }
        public async Task<IActionResult> OnGet(int? id)
        {
            Book = new Book();
            if(id == null)
            {
                return Page();
            }
            Book = await _db.Book.FirstOrDefaultAsync(u => u.Id == id);
            if(Book == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                if(Book.Id == 0)
                {
                    _db.Book.Add(Book);  
                }
                else
                {
                    _db.Book.Update(Book); //  this update is used only when all the property needs to be updated else use below code
                    //var BookFromDb = await _db.Book.FindAsync(Book.Id);
                    //BookFromDb.Name = Book.Name;
                    //BookFromDb.ISBN = Book.ISBN;
                    //BookFromDb.Author = Book.Author;
                }      
                await _db.SaveChangesAsync();
                return RedirectToPage("Index");
            }         
                return RedirectToPage();           
        }
    }
}