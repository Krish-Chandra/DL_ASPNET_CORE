using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL_Core_WebAPP_Release.Areas.Admin.Models
{
    public class AdminUserCreateViewModel
    {
        //[Required]
        //[EmailAddress]
        //[Display(Name = "Email")]
        public string UserName { get; set; }

        //[Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string Role { get; set; }
    }

    public class AdminUserViewModel
    {
        public AdminUserViewModel()
        {

        }
        public AdminUserViewModel(string userName)
        {
            UserName = userName;
        }
        //[Required]
        //[Display(Name = "Email")]
        //[EmailAddress]
        //public string Email { get; set;}

        public string Id { get; set; }
        public string UserName { get;}
        public string Role { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        //public string Password { get; set; }


    }
}
