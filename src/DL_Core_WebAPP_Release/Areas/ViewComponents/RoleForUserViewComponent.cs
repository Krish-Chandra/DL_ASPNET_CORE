using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL_Core_WebAPP_Release.Areas.ViewComponents
{
    public class RoleForUserViewComponent : ViewComponent 
    {
        private UserManager<IdentityUser> _userManager { get; set; }


        public RoleForUserViewComponent([FromServices]UserManager<IdentityUser> um)
        {
            _userManager = um;
        }
        public async Task<string> InvokeAsync(IdentityUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (roles != null)
                return roles[0];
            else
                return string.Empty;
        }
    }
}
