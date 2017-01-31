using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL_Core_WebAPP_Release.Areas.Admin.TagHelpers
{
    [HtmlTargetElement("td", Attributes = "role-name, user-name")]
    public class TdTagHelper : TagHelper
    {
        public string RoleName { get; set; }
        public string UserName { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if ((String.Compare(RoleName, "Administrator", true) == 0) || (String.Compare(RoleName, "Member", true) == 0) || (String.Compare(UserName, "admin@example.com", true) == 0))
                output.Content.Clear();
        }
    }
}
