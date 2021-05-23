using Merchant.Hubs;
using Merchant.Models;
using Merchant.ViewModels;
using Models.DBContext;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Merchant.Controllers
{
    [Authorize(Roles = "restaurant")]
    public class OrderController : Controller
    {
        private FoodDeliveryContext db = new FoodDeliveryContext();

        // GET: Food
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Get()
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["FoodConnection"].ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(@"SELECT [order_id],[order_status] FROM [dbo].[Order]", connection))
                {
                    // Make sure the command object does not already have
                    // a notification object associated with it.
                    command.Notification = null;

                    SqlDependency dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    short userId = (short)Session["userId"];

                    SqlDataReader reader = command.ExecuteReader();
                    var listOrder = db.Orders.Where(x => x.User_order.Any(uo => uo.user_id == userId)).OrderByDescending(x => x.order_created_date);
                    var listPending = listOrder.Where(x => x.order_status.Contains("Pending"));
                    var listConfirmed = listOrder.Where(x => x.order_status.Contains("Order confirmed"));
                    var listShipping = listOrder.Where(x => x.order_status.Contains("Shipping"));
                    var listCompleted = listOrder.Where(x => x.order_status.Contains("Completed"));
                    var listCanceled = listOrder.Where(x => x.order_status.Contains("Order canceled"));

                    return Json(new {
                        ListOrder = JsonConvert.SerializeObject(listOrder),
                        ListPending = JsonConvert.SerializeObject(listPending),
                        ListConfirmed = JsonConvert.SerializeObject(listConfirmed),
                        ListShipping = JsonConvert.SerializeObject(listShipping),
                        ListCompleted = JsonConvert.SerializeObject(listCompleted),
                        ListCanceled = JsonConvert.SerializeObject(listCanceled)
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        public JsonResult getOrderById(short id)
        {
            var Order = db.Orders.Find(id);
            var idUsers = Order.User_order.Where(x => x.user_order_pay != 1);
            var listUser = new List<User>();
            foreach(var item in idUsers)
            {
                listUser.Add(db.Users.Find(item.user_id));
            }
            
            var orderDetail = db.Order_detail.Where(x => x.Order_detail_order_id == Order.order_id);
            var listFood = new List<FoodViewModel>();
            foreach(var item in orderDetail)
            {
                listFood.Add(new FoodViewModel
                {
                    Food = item.Food,
                    Quantity = (short)item.Order_detail_quantity
                });
            }
            return Json(new { 
                Order = JsonConvert.SerializeObject(Order),
                listUser = JsonConvert.SerializeObject(listUser),
                listFood = JsonConvert.SerializeObject(listFood)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getOrderByDate(DateTime? fromDate, DateTime? toDate)
        {
            var listOrder = db.Orders.Where(s => s.order_created_date >= fromDate && s.order_created_date <= toDate);
            var listOrderSuccess = db.Orders.Where(s => s.order_status.Contains("Completed") && s.order_created_date >= fromDate && s.order_created_date <= toDate);
            var listCanceled = db.Orders.Where(s => s.order_status.Contains("Order canceled") && s.order_created_date >= fromDate && s.order_created_date <= toDate);

            return Json(new
            {
                listOrder = JsonConvert.SerializeObject(listOrder),
                listOrderSuccess = JsonConvert.SerializeObject(listOrderSuccess),
                listCanceled = JsonConvert.SerializeObject(listCanceled)
            }, JsonRequestBehavior.AllowGet);
        }

		[HttpGet]
		public JsonResult getListFoodByOrderDate(DateTime? fromDate, DateTime? toDate)
		{
            var list = from od in db.Order_detail
                       join o in db.Orders on od.Order_detail_order_id equals o.order_id
                       join f in db.Foods on od.Order_detail_food_id equals f.food_id
                       where o.order_created_date >= fromDate && o.order_created_date <= toDate
                       group od by new { f.food_name, o.order_created_date } into grouped
                       select new FoodModel
                       {
                           FoodName = grouped.Key.food_name,
                           DateOrder = (DateTime)grouped.Key.order_created_date,
                           Quantity = (short)grouped.Sum(p => p.Order_detail_quantity)
                       };
			return Json(new
			{
				listOrderByDate = JsonConvert.SerializeObject(list),
			}, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
        public void changeStatus(short id ,string status)
        {
            var order = db.Orders.Find(id);
            order.order_status = status;
            db.SaveChanges();
        }

        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if(e.Type == SqlNotificationType.Change)
            {
                OrderHub.Show();
            }
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