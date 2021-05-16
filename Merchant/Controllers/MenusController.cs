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
    public class MenusController : Controller
    {
        private FoodDeliveryContext db = new FoodDeliveryContext();

        // GET: Menus
        public ActionResult Index()
        {
            short userId = (short)Session["userId"];
            var restaurant = db.Restaurants.Where(x => x.restaurant_user_id == userId).FirstOrDefault();
            var menus = db.Menus.Where(x => x.menu_restaurant_id == restaurant.restaurant_id);
            return View(menus.ToList());
        }

        // GET: Menus/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // GET: Menus/Create
        public ActionResult Create()
        {
            ViewBag.menu_restaurant_id = new SelectList(db.Restaurants, "restaurant_id", "restaurant_name");
            return View();
        }

        // POST: Menus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "menu_id,menu_name,menu_restaurant_id")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                short userId = (short)Session["userId"];
                var restaurant = db.Restaurants.Where(x => x.restaurant_user_id == userId).FirstOrDefault();
                menu.menu_restaurant_id = restaurant.restaurant_id;
                db.Menus.Add(menu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menu);
        }

        // GET: Menus/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            ViewBag.menu_restaurant_id = new SelectList(db.Restaurants, "restaurant_id", "restaurant_name", menu.menu_restaurant_id);
            return View(menu);
        }

        // POST: Menus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "menu_id,menu_name,menu_restaurant_id")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.menu_restaurant_id = new SelectList(db.Restaurants, "restaurant_id", "restaurant_name", menu.menu_restaurant_id);
            return View(menu);
        }

        // POST: Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(short id)
        {
            Menu menu = db.Menus.Find(id);
            db.Menus.Remove(menu);
            db.SaveChanges();
            return RedirectToAction("Index");
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
