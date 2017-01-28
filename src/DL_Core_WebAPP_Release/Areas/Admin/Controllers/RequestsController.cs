using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DL_Core_WebAPP_Release.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DL_Core_WebAPP_Release.Areas.Admin.Controllers
{
    [Authorize(Policy = "ManageRequests")]
    public class RequestsController : AdminController
    {
        private DLContext db;
        private UserManager<IdentityUser> _userManager { get; set; }

        public RequestsController(DLContext context, [FromServices]UserManager<IdentityUser> um)
        {
            db = context;
            _userManager = um;
        }


        // GET: Admin/Requests
        public async Task<ActionResult> Index()
        {
            var requests = db.Requests.Include(r => r.Book);
            return View(await requests.ToListAsync());
        }

        //[Authorize]
        public ActionResult IssueBook(int Id)
        {
            IssueBookToUser(Id);
            return RedirectToAction("Index");
        }

        private bool IssueBookToUser(int Id)
        {
            Boolean retVal = false;
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    //Get the request
                    Request BookReq = db.Requests.Single(req => req.RequestID == Id);
                    if (BookReq != null)
                    {
                        string issueDate = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                        //Create an Issue for the request
                        DateTime dueOn = DateTime.Parse(issueDate).AddDays(14);
                        string dueDate = dueOn.Day + "/" + dueOn.Month + "/" + dueOn.Year;

                        Issue Issue = new Issue() { BookId = BookReq.BookId, UserId = BookReq.UserId, IssuedOn =  issueDate, DueOn = dueDate  };
                        if (db.Issues.Add(Issue) != null)
                        {
                            Book ReqBook = db.Books.Single(book => book.BookId == BookReq.BookId);
                            if (ReqBook != null)
                            {
                                db.Books.Attach(ReqBook);
                                ReqBook.AvailableCopies--;
                                //If the issue is successfully added, remove the request
                                if (db.Requests.Remove(BookReq) != null)
                                {
                                    db.SaveChanges();
                                    retVal = true;
                                }
                            }
                        }
                    }
                    if (retVal)
                        dbContextTransaction.Commit();
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    retVal = false;
                }
            }
            return retVal;
        }


        // GET: Admin/Requests/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Request request = await db.Requests.SingleAsync(req => req.RequestID == id);
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }

        // POST: Admin/Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Request request = await db.Requests.SingleAsync(req => req.RequestID == id);
            db.Requests.Remove(request);
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
