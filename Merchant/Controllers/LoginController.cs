using Models.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Merchant.Controllers
{
    public class LoginController : Controller
    {
        private FoodDeliveryContext db = new FoodDeliveryContext();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            var checkLogin = db.Users.Where(x => x.user_email == user.user_email && x.user_password == user.user_password).FirstOrDefault();
            if(checkLogin != null)
            {
                if(!checkLogin.Role.role_description.Equals("restaurant"))
                {
                    ViewBag.Notification = "Quyền gì bạn ơi!";
                    return View();
                }
                FormsAuthentication.SetAuthCookie(checkLogin.user_name, false);
                Session["userId"] = checkLogin.user_id;
                Session["userImg"] = checkLogin.user_image;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Notification = "Sai tên đăng nhập hoặc mật khẩu!";
            }
            return View();
        }

        [Authorize]
        public ActionResult SignOut()
        {
            Session.RemoveAll();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Login");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}