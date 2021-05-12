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
    public class RestaurantsController : Controller
    {
        private FoodDeliveryContext db = new FoodDeliveryContext();
        [Authorize(Roles = "restaurant")]
        public ActionResult Edit()
        {
            short idUser = (short)Session["userId"];
            Restaurant restaurant = db.Restaurants.Where(x => x.restaurant_user_id == idUser).FirstOrDefault();
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            ViewBag.restaurant_category_id = new SelectList(db.Restaurant_category, "restaurant_category_id", "restaurant_category_name", restaurant.restaurant_category_id);
            return View(restaurant);
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "restaurant_id,restaurant_name,restaurant_image,restaurant_address,restaurant_phone,restaurant_email,restaurant_start_time,restaurant_end_time,restaurant_longitude,restaurant_latitude,restaurant_user_id,restaurant_category_id")] Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                restaurant.restaurant_user_id = (short?)Session["userId"];
                db.Entry(restaurant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit");
            }
            ViewBag.restaurant_category_id = new SelectList(db.Restaurant_category, "restaurant_category_id", "restaurant_category_name", restaurant.restaurant_category_id);
            return View(restaurant);
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
