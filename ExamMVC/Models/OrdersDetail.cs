using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamMVC.Models
{
    public class OrdersDetail
    {
        public string OrderID { get; set; }
        public string ProductID { get; set; }
        public string UnitPrice { get; set; }
        public string Quantity { get; set; }
        public string Discount { get; set; }
    }
}