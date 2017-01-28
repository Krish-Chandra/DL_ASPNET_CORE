//Step 16 - 
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DL_Using_DotNet_Core.Infrastructure;
using DL_Core_WebAPP_Release.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DL_Using_DotNet_Core.Controllers
{

    public class LibraryController : Controller
    {
        private LibraryRepository _repo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DLContext _DLContext;


        public LibraryController(LibraryRepository repo, UserManager<IdentityUser> userManager, DLContext dlContext)
        {
            _repo = repo;
            _userManager = userManager;
            _DLContext = dlContext;
        }

        public IActionResult Index()
        {
            return View(_repo.GetAll());
        }

        [HttpPost]
        public ActionResult AddToCart(int Id)
        {
            List<int> bookIds = HttpContext.Session.GetObjectFromJson<List<int>>("BOOK_IDS");
            if (bookIds != null)
            {
                if (bookIds.IndexOf(Id) == -1)
                {
                    bookIds.Add(Id);
                }

            }
            else
            {
                bookIds = new List<int>();
                bookIds.Add(Id);
          }
            HttpContext.Session.SetObjectAsJson("BOOK_IDS", bookIds);
            TempData.Add("MSG", "The selected book has been successfully added to your request cart!");
            return RedirectToAction("Index");
        }
        public ActionResult DisplayViewCart()
        {
            List<Book> reqBooks = null;
            List<int> bookIds = HttpContext.Session.GetObjectFromJson<List<int>>("BOOK_IDS");
            if (bookIds != null)
            {
                reqBooks = new List<Book>();
                foreach (int id in bookIds)
                {
                    reqBooks.Add(_repo.Get(id));
                }

            }
            return View(reqBooks);
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int Id)
        {
            List<int> bookIds = HttpContext.Session.GetObjectFromJson<List<int>>("BOOK_IDS");
            if (bookIds != null)
            {
                if (bookIds.IndexOf(Id) != -1)
                {
                    if (bookIds.Remove(Id))
                    {
                        HttpContext.Session.SetObjectAsJson("BOOK_IDS", bookIds);
                        TempData.Add("MSG", "The selected book has been successfully removed from your request cart!");
                    }
                    else
                    {
                        TempData.Add("MSG", "Couldn't remove the book from your request cart!");
                    }

                }
            }
            return RedirectToAction("DisplayViewCart");
        }

        [Authorize]
        //If the user tries to checkout without loggin in, he will be asked to login and , if successful, will be redirected to this action
        //So accept GET as well
        [AcceptVerbs("POST", "GET")] 
        public async Task<ActionResult> Checkout()
        {

            List<int> bookIds = HttpContext.Session.GetObjectFromJson<List<int>>("BOOK_IDS");
            string Msg = string.Empty;
            if (bookIds == null)
            {
                ViewBag.Message = "Your Request Cart is empty! Add books to your cart first!!";
            }
            else
            {
                int Count = bookIds.Count;
                //List<String> Titles;
                int Total = await  ConfirmRequests();
                if (Count == Total)
                    Msg = "All your requests have been processed successfully!";
                else
                {
                    Msg = "Couldn't process ALL your requests!. Successfully processed the following requests:";
                }
                TempData["MSG"] = Msg;

            }
            return RedirectToAction("Index");
        }

        private async Task<int> ConfirmRequests()
        {
            IdentityUser user = await _userManager.FindByNameAsync(Request.HttpContext.User.Identity.Name);
            string userid = user.Id;
            int successCount = 0;
            List<int> bookIds = HttpContext.Session.GetObjectFromJson<List<int>>("BOOK_IDS");
            List<int> requests = new List<int>();
            for (int i = 0; i < bookIds.Count; i++)
            {
                Request req = new Request { BookId = bookIds[i], RequesedOn = DateTime.Now.ToString("d"), UserId = userid };
                _DLContext.Requests.Add(req);
                if (_DLContext.SaveChanges() > 0)
                {
                    requests.Add(bookIds[i]);
                    successCount++;
                }
            }
            bookIds.RemoveAll(bookId => requests.Contains(bookId));
            HttpContext.Session.SetObjectAsJson("BOOK_IDS", bookIds);
            return successCount;
        }
    }
}
