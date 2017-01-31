using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL_Core_WebAPP_Release.Areas.Admin.Models
{
    public class AdminUserCreateViewModel
    {
        [Required(ErrorMessage = "Please enter the emailID of the user") ]
        [Display(Name = "Email ID")]
        [EmailAddress(ErrorMessage = "Please enter a valid email ID")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Please select a valid role")]
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

        //public string Email { get; set;}

        public string Id { get; set; }

        [Required(ErrorMessage="Please enter the emailID of the user") ]
        [Display(Name = "Email ID")]
        [EmailAddress(ErrorMessage ="Please enter a valid email ID")]
        public string UserName { get;}

        [Required(ErrorMessage ="Please select a valid role")]
        public string Role { get; set; }

    }
}
