using DL_Core_WebAPP_Release.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading.Tasks;

namespace DL_Core_WebAPP_Release.Areas.Admin.Controllers
{
    [Authorize(Policy = "ManageAuthors")]
    public class AuthorsController : AdminController
    {
        private DLContext db;

        public AuthorsController(DLContext context)
        {
            db = context;
        }

        // GET: Admin/Authors
        public async Task<ActionResult> Index()
        {
            return View(await db.Authors.ToListAsync());
        }


        // GET: Admin/Authors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Authors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("AuthorID,Name,Address,City,State,PinCode,Email,Phone" )] Author author)
        {
            if (ModelState.IsValid)
            {
                db.Authors.Add(author);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(author);
        }

        // GET: Admin/Authors/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(404);
            }
            Author author = await db.Authors.SingleAsync(auth => auth.AuthorID == id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Admin/Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(int? authorId)
        {
            if (authorId == null)
            {
                return NotFound();
            }
            var authorToUpdate = await db.Authors.SingleOrDefaultAsync(a => a.AuthorID == authorId);
            if (await TryUpdateModelAsync<Author>(authorToUpdate, "", auth => auth.Name, auth => auth.Address, auth=>auth.City, auth => auth.State, auth => auth.PinCode, auth => auth.Email, auth => auth.Phone))
            {
                try
                {
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

            return View(authorToUpdate);
        }

        private bool AuthorExists(int authorId)
        {
            var author = db.Authors.FirstAsync(auth => auth.AuthorID == authorId);
            return (author != null) ? true : false;
        }

        // GET: Admin/Authors/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(404);
            }
            Author author = await db.Authors.SingleAsync(auth => auth.AuthorID == id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Admin/Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Author author = await db.Authors.SingleAsync(auth => auth.AuthorID == id);
            db.Authors.Remove(author);
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
