using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace DL_Core_WebAPP_Release.Helpers
{
    public class ViewHelper
    {
        public static string GetString(IHtmlContent content)
        {
            var writer = new System.IO.StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            string result = writer.ToString();
            return String.IsNullOrEmpty(result) ? "No Name" : result;
        }
    }
}
