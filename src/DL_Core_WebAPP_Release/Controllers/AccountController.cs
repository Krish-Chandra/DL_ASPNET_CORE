using DL_Core_WebAPP_Release.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;


namespace DL_Core_WebAPP_Release.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private UserManager<IdentityUser> UserManager { get; set; }

        private SignInManager<IdentityUser> SignInManager { get; set; }

        public AccountController([FromServices]UserManager<IdentityUser> um, [FromServices]SignInManager<IdentityUser> sm)
        {
            UserManager = um;
            SignInManager = sm;
        }
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to lockoutOnFailure: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToLocal(returnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
            }
            if (result.IsLockedOut)
            {
                return View("Lockout");
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }
        }



        //
        // GET: /Account/Register
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //Bug: Remember browser option missing?
                    //Uncomment this and comment the later part if account verification is not needed.
                    await SignInManager.SignInAsync(user, isPersistent: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    //string code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    //await MessageServices.SendEmailAsync(model.Email, "Confirm your account",
                    //    "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
#if !DEMO
                    return RedirectToAction("Index", "Home");
#else
                    //To display the email link in a friendly page instead of sending email
                    ViewBag.Link = callbackUrl;
                    return View("DemoLinkDisplay");
#endif
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }



        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogOff()
        {
            // clear all items from the cart
            HttpContext.Session.Clear();

            await SignInManager.SignOutAsync();
            return RedirectToAction("Index", "library");
        }


        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        private async Task<IdentityUser> GetCurrentUserAsync()
        {
            return await UserManager.FindByNameAsync(HttpContext.User.Identity.Name);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Library");
            }
        }

        #endregion
    }
}
