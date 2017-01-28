using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DL_Core_WebAPP_Release.Models;
using DL_Core_WebAPP_Release.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace DL_Core_WebAPP_Release.Areas.Admin.Controllers
{
    [Authorize(Policy = "ManageCategories")]
    public class CategoriesController : AdminController
    {
        private DLContext db;

        public CategoriesController(DLContext context)
        {
            db = context;
        }

        // GET: Admin/Categories
        public async Task<ActionResult> Index()
        {
            return View(await db.Categories.ToListAsync());
        }

        // GET: Admin/Categories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Category category = await db.Categories.SingleAsync(cat => cat.CategoryID == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // GET: Admin/Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("CategoryID,Name,Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Admin/Categories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Category category = await db.Categories.SingleAsync(cat => cat.CategoryID == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(int? categoryId)
        {
            if (categoryId == null)
            {
                return NotFound();
            }
            var catToUpdate = await db.Categories.SingleOrDefaultAsync(c => c.CategoryID == categoryId);
            if (await TryUpdateModelAsync<Category>(catToUpdate, "", cat=>cat.Name, cat=>cat.Description))
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

            return View(catToUpdate);
        }

        private bool CategoryExists(int categoryId)
        {
            var category = db.Categories.FirstAsync(cat => cat.CategoryID == categoryId);
            return (category != null) ? true : false;
        }

        // GET: Admin/Categories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Category category = await db.Categories.SingleAsync(cat => cat.CategoryID == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Category category = await db.Categories.SingleAsync(cat => cat.CategoryID == id);
            db.Categories.Remove(category);
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
