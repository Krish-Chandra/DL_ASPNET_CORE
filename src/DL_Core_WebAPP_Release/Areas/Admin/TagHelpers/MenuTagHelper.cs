using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DL_Core_WebAPP_Release.Areas.Admin.TagHelpers
{
    public class MenuTagHelper : TagHelper
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ClaimsPrincipal User { get; set; }

        public MenuTagHelper(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "ul";
            output.Attributes.Add("class", "nav navbar-nav");
            output.TagMode = TagMode.StartTagAndEndTag;
            IEnumerable<Claim> permissions = User.Claims.Where(c => c.Value == "Allowed");
            string li = string.Empty;
            foreach (var perm in permissions)
            {
                li += $@"<li><a href=""/admin/{perm.Type}"">{perm.Type}</a></li>";
            }

            output.Content.SetHtmlContent(li);
        }
    }
}
