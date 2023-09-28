using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamMVC.Models
{
    public class OrdersModel
    {
        public string OrderID { get; set; }
        public string OrderDate { get; set; }
        public string CustomerID { get; set; }
        public string EmployeeID { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ProductID { get; set; }
        public string UnitPrice { get; set; }
        public string Quantity { get; set; }
        public string Discount { get; set; }
    }
}