using DL_Core_WebAPP_Release.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DL_Core_WebAPP_Release.Areas.Admin.Controllers
{

    [Authorize(Policy = "ManageRoles")]
    public class RolesController : AdminController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public ViewResult Index()
        {
            return View(_roleManager.Roles.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Authors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create(tring roleName, string[] permissions)
        public async Task<ActionResult> Create( [Bind("Name, Claims")] Role role)
        {
            if (ModelState.IsValid)
            {
                var newRole = new IdentityRole(role.Name);
                var result  = await _roleManager.CreateAsync(newRole);
                if (result.Succeeded)
                {
                    foreach(String perm in role.Claims)
                    {
                        await _roleManager.AddClaimAsync(newRole, new Claim(perm, "Allowed"));
                    }
                }
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: Admin/Roles/Edit/id
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            IdentityRole role = _roleManager.Roles.SingleOrDefault(rol => rol.Id == id);
            var roleClaims = await _roleManager.GetClaimsAsync(role);
            ViewBag.roleClaims = roleClaims.Select(claim => claim.Type).ToArray();
            if (role == null)
            {
                return NotFound();
            }
            return View(new Role { Id=role.Id, Name=role.Name});
        }

        // POST: Admin/Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Role roleToUpdate)
        {
            if (String.IsNullOrEmpty(roleToUpdate.Id) || String.IsNullOrWhiteSpace(roleToUpdate.Id) || !RoleExists(roleToUpdate.Id))
            {
                return new StatusCodeResult(404);
            }
            IdentityRole role = _roleManager.Roles.SingleOrDefault(rol => rol.Id == roleToUpdate.Id);
            var roleClaims = await _roleManager.GetClaimsAsync(role);
            if (role == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    foreach (var claim in roleClaims)
                    {
                        await _roleManager.RemoveClaimAsync(role, claim);
                    }

                    foreach (var newClaim in roleToUpdate.Claims)
                    {
                        await _roleManager.AddClaimAsync(role, new System.Security.Claims.Claim(newClaim, "Allowed"));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction("Index");
            }

            ViewBag.roleClaims = roleClaims.Select(claim => claim.Type).ToArray();
            return View(role);
        }

        private bool RoleExists(string id)
        {
            IdentityRole role = _roleManager.Roles.SingleOrDefault(rol => rol.Id == id);
            return (role != null) ? true : false;
        }

        public async Task<ActionResult> Delete(string id)
        {
            if ((id == null) || !RoleExists(id))
            {
                return NotFound();
            }

            IdentityRole role = await _roleManager.Roles.SingleAsync(rol => rol.Id == id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            IdentityRole role = await _roleManager.Roles.SingleAsync(rol => rol.Id == id);
            await _roleManager.DeleteAsync(role);
            return RedirectToAction("Index");
        }
    }
}
