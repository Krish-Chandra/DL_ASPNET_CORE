using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DL_Core_WebAPP_Release.Models.Services
{
    public class UserNameFromUserIdService
    {
        private UserManager<IdentityUser> _userManager { get; set; }


        public UserNameFromUserIdService([FromServices]UserManager<IdentityUser> um)
        {
            _userManager = um;
        }
        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
                return user.UserName.ToUpper();
            else
                return string.Empty;
        }

    }
}
