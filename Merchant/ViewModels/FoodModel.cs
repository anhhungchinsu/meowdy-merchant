using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Merchant.ViewModels
{
	public class FoodModel
	{
		public string FoodName { get; set; }
		public short Quantity { get; set; }
		public DateTime DateOrder { get; set; } 
	}
}