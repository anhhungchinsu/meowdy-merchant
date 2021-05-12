using Models.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Merchant.Models
{
    public class FoodViewModel
    {
        public Food Food { get; set; }
        public short Quantity { get; set; }
    }
}