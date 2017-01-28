using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL_Core_WebAPP_Release.Areas.Admin.Controllers
{
    public class HomeController : AdminController
    {
        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}
