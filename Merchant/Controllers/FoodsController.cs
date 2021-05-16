using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models.DBContext;
using Newtonsoft.Json;

namespace Merchant.Controllers
{
    public class FoodsController : Controller
    {
        private FoodDeliveryContext db = new FoodDeliveryContext();

        // GET: Foods
        public ActionResult ListFoodByMenu(short id)
        {
            var foods = db.Foods.Where(x => x.food_menu_id == id);
            ViewBag.food_menu_id = id;
            return PartialView(foods.ToList());
        }

        public JsonResult ListFoodByMenu2(short id)
        {
            var listFood = db.Foods.Where(x => x.food_menu_id == id);
            return Json(new
            {
                listFood = JsonConvert.SerializeObject(listFood)
            }, JsonRequestBehavior.AllowGet);
        }

        // GET: Foods/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = db.Foods.Find(id);
            if (food == null)
            {
                return HttpNotFound();
            }
            return PartialView(food);
        }

        // GET: Foods/Create
        public ActionResult Create()
        {
            ViewBag.food_menu_id = new SelectList(db.Menus, "menu_id", "menu_name");
            return View();
        }

        // POST: Foods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "food_id,food_name,food_price,food_quantity,food_image,food_served,food_note,food_menu_id")] Food food)
        {
            if (ModelState.IsValid)
            {
                db.Foods.Add(food);
                db.SaveChanges();
                return RedirectToAction("Index","Menus");
            }

            ViewBag.food_menu_id = new SelectList(db.Menus, "menu_id", "menu_name", food.food_menu_id);
            return View(food);
        }

        // GET: Foods/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = db.Foods.Find(id);
            if (food == null)
            {
                return HttpNotFound();
            }
            ViewBag.food_menu_id = new SelectList(db.Menus, "menu_id", "menu_name", food.food_menu_id);
            return View(food);
        }

        // POST: Foods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "food_id,food_name,food_price,food_quantity,food_image,food_served,food_note,food_menu_id")] Food food)
        {
            if (ModelState.IsValid)
            {
                db.Entry(food).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Menus");
            }
            return RedirectToAction("Index", "Menus");
        }

        // POST: Foods/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(short id)
        {
            Food food = db.Foods.Find(id);
            db.Foods.Remove(food);
            db.SaveChanges();
            return RedirectToAction("Index", "Menus");
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
