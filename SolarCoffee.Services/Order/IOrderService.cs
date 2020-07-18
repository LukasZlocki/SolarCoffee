using System.Collections.Generic;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Services.Order
{
    public interface IOrderService
    {
         public List<SalesOrder> GetOrders();
         ServiceResponse<bool> GenerateInvoiceForOrder(SalesOrder order);
         ServiceResponse<bool> MarkFullfilled(int id);

    }
}