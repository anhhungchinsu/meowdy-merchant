//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Models.DBContext
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    
    public partial class User_order
    {
        public short user_id { get; set; }
        public short order_id { get; set; }
        public Nullable<System.DateTime> user_order_created_date { get; set; }
        public Nullable<decimal> user_order_pay { get; set; }
        [JsonIgnore]
        public virtual Order Order { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
