using DL_Core_WebAPP_Release.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL_Core_WebAPP_Release.Areas.Admin.Controllers
{
    [Authorize(Policy = "ManagePublishers")]
    public class PublishersController : AdminController
    {
        private DLContext db;

        public PublishersController(DLContext context)
        {
            db = context;
        }

        // GET: Admin/Publishers
        public async Task<ActionResult> Index()
        {
            return View(await db.Publishers.ToListAsync());
        }

        // GET: Admin/Publishers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(404);
            }
            Publisher publisher= await db.Publishers.SingleAsync(pub => id == pub.PublisherID);
            if (publisher == null)
            {
                return NotFound();
            }
            return View(publisher);
        }

        // GET: Admin/Publishers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Publishers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("PublisherID,Name,Address,City,State,PinCode,Email,Phone")] Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                db.Publishers.Add(publisher);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(publisher);
        }

        // GET: Admin/Publishers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(404);
            }
            Publisher publisher = await db.Publishers.SingleAsync(pub => pub.PublisherID == id);
            if (publisher == null)
            {
                return NotFound();
            }
            return View(publisher);
        }

        // POST: Admin/Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(int? publisherId)
        {
            if (publisherId == null)
            {
                return NotFound();
            }
            var pubToUpdate = await db.Publishers.SingleOrDefaultAsync(p => p.PublisherID == publisherId);
            if (await TryUpdateModelAsync<Publisher>(pubToUpdate, "", pub=>pub.Name, pub=>pub.Address, pub=>pub.City, pub=>pub.State, pub => pub.PinCode, pub => pub.Email, pub => pub.Phone))
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

            return View(pubToUpdate);
        }

        private bool PublisherExists(int publisherId)
        {
            var publisher = db.Publishers.FirstAsync(pub => pub.PublisherID == publisherId);
            return (publisher != null) ? true : false;
        }

        // GET: Admin/Authors/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(404);
            }
            Publisher publisher = await db.Publishers.SingleAsync(pub => pub.PublisherID == id);
            if (publisher == null)
            {
                return NotFound();
            }
            return View(publisher);
        }

        // POST: Admin/Publishers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Publisher publisher = await db.Publishers.SingleAsync(pub => pub.PublisherID == id);
            db.Publishers.Remove(publisher);
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
