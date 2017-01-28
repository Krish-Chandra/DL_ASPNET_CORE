using DL_Core_WebAPP_Release.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DL_Core_WebAPP_Release.Areas.ViewComponents
{
    public class UserNameFromIdViewComponent : ViewComponent
    {
        private UserManager<IdentityUser> _userManager { get; set; }


        public UserNameFromIdViewComponent([FromServices]UserManager<IdentityUser> um)
        {
            _userManager = um;
        }
        public async Task<string> InvokeAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
                return user.UserName;
            else
                return string.Empty;
        }
    }
}
