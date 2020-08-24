using System;
using System.Collections.Generic;

namespace SolarCoffee.Web.ViewModels
{
    /// View model for open SalesOrders
    
    public class InvoiceModel {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedON { get; set; }
        public int CustomerId { get; set; }
        public List<SalesOrderItemModel> LineItems  { get; set; }
        
    }

    public class SalesOrderItemModel {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public ProductModel Product {get; set;  }
    }
}