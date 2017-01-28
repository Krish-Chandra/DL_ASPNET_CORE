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
        public async Task<ActionResult> Create( [Bind("Name, Permissions")] Role role)
        {
            if (ModelState.IsValid)
            {
                var newRole = new IdentityRole(role.Name);
                var result  = await _roleManager.CreateAsync(newRole);
                if (result.Succeeded)
                {
                    foreach(String perm in role.Permissions)
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
                return new StatusCodeResult(404);
            }
            IdentityRole role = _roleManager.Roles.SingleOrDefault(rol => rol.Id == id);
            var roleClaims = await _roleManager.GetClaimsAsync(role);
            ViewBag.roleClaims = roleClaims.Select(claim => claim.Type).ToArray();
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // POST: Admin/Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, string[] claims)
        {
            if (String.IsNullOrEmpty(id) || String.IsNullOrWhiteSpace(id))
            {
                return new StatusCodeResult(404);
            }
            IdentityRole role = _roleManager.Roles.SingleOrDefault(rol => rol.Id == id);
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

                    foreach (var newClaim in claims)
                    {
                        await _roleManager.AddClaimAsync(role, new System.Security.Claims.Claim(newClaim, "Allowed"));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
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
    }
}
