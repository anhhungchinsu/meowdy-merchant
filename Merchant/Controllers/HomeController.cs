using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Merchant.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles = "restaurant")]
        public ActionResult Index()
        {
            return View();
        }
    }
}