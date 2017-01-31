using DL_Core_WebAPP_Release.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DL_Core_WebAPP_Release.Areas.Admin.Controllers
{
    [Authorize(Policy = "ManageAdminUsers")]
    public class AdminUsersController : AdminController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminUsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            const string memberRoleName = "Member";
            IdentityRole memberRole = _roleManager.Roles.Where(r => r.Name == memberRoleName).FirstOrDefault();
            var query = _userManager.Users;
            if (memberRole != null)
            {
                query = query.Where(u => !u.Roles.Select(r => r.RoleId).Contains(memberRole.Id));
            }
            return View(query.ToList());
        }

        public IActionResult Create()
        {
            ViewBag.Roles = _roleManager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name });
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminUserCreateViewModel userToCreate)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser {UserName = userToCreate.UserName, Email = userToCreate.UserName };
                var result = await _userManager.CreateAsync(user, userToCreate.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, userToCreate.Role);
                    return RedirectToAction("Index");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            ViewBag.Roles = _roleManager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name });
            return View(userToCreate);
        }

        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.Roles = _roleManager.Roles.Select(r => new SelectListItem { Value=r.Name, Text=r.Name});
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            else
            {
                var roles = await _userManager.GetRolesAsync(user);
                string role = roles.Count > 0 ? roles[0] : "";
                AdminUserViewModel adminVM = new AdminUserViewModel(user.UserName) { Id=user.Id, Role = role};
                return View(adminVM);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string id, AdminUserViewModel vm)
        {
            if (id != vm.Id)
                return BadRequest();

            var user = await _userManager.FindByIdAsync(id);
            IList<string> rolesForUser = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, rolesForUser);
            await _userManager.AddToRoleAsync(user, vm.Role);
            return RedirectToAction("index");
        }

        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new StatusCodeResult(404);
            }
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

    }
}
