using DL_Core_WebAPP_Release.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace DL_Core_WebAPP_Release.Areas.Admin.Controllers
{
    [Authorize(Policy ="ManageBooks")]
    public class BooksController : AdminController
    {
        private DLContext db;

        public BooksController(DLContext context)
        {
            db = context;
        }

        // GET: Admin/Books
        public ActionResult Index()
        {
            var books = db.Books.Include(b => b.Author).Include(b => b.Category).Include(b => b.Publisher);
            return View(books);
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(404);
            }

            Book book = await db.Books.Include(b => b.Author).Include(b => b.Category).Include(b => b.Publisher).FirstOrDefaultAsync(b => b.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }


        public ActionResult Create()
        {
            ViewBag.Authors = new SelectList(db.Authors, "AuthorID", "Name");
            ViewBag.Categories = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.Publishers = new SelectList(db.Publishers, "PublisherID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Title,ISBN,PublisherId,TotalCopies,AuthorId,CategoryId")] Book book)
        {
            if (ModelState.IsValid)
            {
                book.AvailableCopies = book.TotalCopies; //The number of available copies will initially be equal to the total copies of the book
                db.Books.Add(book);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Authors = new SelectList(db.Authors, "AuthorID", "Name");
            ViewBag.Categories = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.Publishers = new SelectList(db.Publishers, "PublisherID", "Name");
            return View();
        }
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //Book book = await db.Books.FindAsync(id);
            Book book = await db.Books.Include(b => b.Author).Include(b => b.Category).Include(b => b.Publisher).FirstOrDefaultAsync(b => b.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
            ViewBag.Authors = new SelectList(db.Authors, "AuthorID", "Name");
            ViewBag.Categories = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.Publishers = new SelectList(db.Publishers, "PublisherID", "Name");

            return View(book);
        }

        // POST: Admin/Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        private bool BookExists(int bookId)
        {
            var book = db.Books.FirstAsync(bk => bk.BookId == bookId);
            return (book != null) ? true : false;
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(int? bookId)
        {
            if (bookId == null)
            {
                return NotFound();
            }
            var bookToUpdate = await db.Books.SingleOrDefaultAsync(book => book.BookId == bookId);
            if (await TryUpdateModelAsync<Book>(bookToUpdate, "", book => book.Title, book=>book.AuthorId, book=>book.PublisherId, book=>book.CategoryId, book=>book.ISBN, book=>book.TotalCopies ))
            {
                try
                {
                    bookToUpdate.AvailableCopies = bookToUpdate.TotalCopies;
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction("Index");
            }
            ViewBag.Authors = new SelectList(db.Authors, "AuthorID", "Name");
            ViewBag.Categories = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.Publishers = new SelectList(db.Publishers, "PublisherID", "Name");

            return View(bookToUpdate);
        }

        // GET: Admin/Books/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //Book book = await db.Books.FindAsync(id);
            Book book = await db.Books.Include(b => b.Author).Include(b => b.Category).Include(b => b.Publisher).FirstOrDefaultAsync(b => b.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Admin/Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Book book = await db.Books.SingleAsync(bk => bk.BookId == id);
            db.Books.Remove(book);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}