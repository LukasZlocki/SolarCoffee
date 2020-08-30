using System;
using System.Collections.Generic;
using System.Linq;
using SolarCoffee.Data.Models;
using SolarCoffee.Web.ViewModels;

namespace SolarCoffee.Web.Serialization
{
    // HAndles mapping Order fata models to and from related View Models
    public static class OrderMapper
    {
        

        // Maps an InvoiceModel view model to a SalesOrder data model
        public static SalesOrder SerializeInvoiceToOrder(InvoiceModel invoice) {
            var salesOrderItems = invoice.LineItems
            .Select(item => new SalesOrderItem {
                Id = item.Id,
                Quantity = item.Id,
                Product = ProductMapper.SerializeProductModel(item.Product)
            }).ToList();

            return new SalesOrder {
                SalesOrderItem = salesOrderItems,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow
            };
        }

        // Maps a collection of SalesOrders (data) to OrdersModels (view models)
        public static List<OrderModel> SerializeOrdersToViewModel(IEnumerable<SalesOrder> orders) {
            return orders.Select(order => new OrderModel {
                Id = order.Id,
                CreatedOn = order.CreatedOn,
                UpdatedOn = order.UpdatedOn,
                SalesOrderItem = SerializeSalesOrderItems(order.SalesOrderItem),
                Customer = CustomerMapper.SerializeCustomer(order.Customer),
                IsPaid = order.IsPaid
            }).ToList();
        }

        // Maps a collection of SalesOrderItems (data) to SalesOrderItemModels (view models)
        private static List<SalesOrderItemModel> SerializeSalesOrderItems(IEnumerable<SalesOrderItem> orderItems){
            return orderItems.Select(item => new SalesOrderItemModel {
                Id = item.Id,
                Quantity = item.Quantity,
                Product = ProductMapper.SerializeProductModel(item.Product)
            }).ToList();
        }
    }
}