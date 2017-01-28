using DL_Core_WebAPP_Release.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DL_Core_WebAPP_Release.Areas.Admin.Controllers
{
    [Authorize(Policy = "ManageIssues")]
    public class IssuesController : AdminController
    {
        private DLContext db;

        public IssuesController(DLContext context)
        {
            db = context;
        }

        // GET: Admin/Issues
        public async Task<ActionResult> Index()
        {
            var issues = db.Issues.Include(i => i.Book);
            return View(await issues.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> ReturnBook(int Id)
        {
            await ReturnBookFromUser(Id);
            return RedirectToAction("Index");
        }

        private async Task<bool> ReturnBookFromUser(int Id)
        {
            Boolean retVal = false;
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    //Get the request
                    Issue issue = await db.Issues.SingleAsync(isue => isue.IssueID == Id);
                    if (issue != null)
                    {
                        //Create an Issue for the request
                        if (db.Issues.Remove(issue) != null)
                        {
                            Book book = await db.Books.SingleAsync(bk => bk.BookId == issue.BookId);
                            if (book != null)
                            {
                                db.Books.Attach(book);
                                book.AvailableCopies++;
                                db.SaveChanges();
                                retVal = true;
                            }
                        }
                    }
                    if (retVal)
                        dbContextTransaction.Commit();
                }
                catch(Exception ex)
                {
                    dbContextTransaction.Rollback();
                    retVal = false;
                }
            }
            return retVal;
        }
    }
}
