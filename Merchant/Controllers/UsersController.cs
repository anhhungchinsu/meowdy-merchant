using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models.DBContext;

namespace Merchant.Controllers
{
    [Authorize(Roles = "restaurant")]
    public class UsersController : Controller
    {
        private FoodDeliveryContext db = new FoodDeliveryContext();

        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.user_role_id = new SelectList(db.Roles, "role_id", "role_description", user.user_role_id);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "user_id,user_name,user_email,user_sex,user_password,user_image,user_phone,user_address")] User user)
        {
            if (ModelState.IsValid)
            {
                user.user_role_id = 4;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit");
            }
            return View(user);
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
