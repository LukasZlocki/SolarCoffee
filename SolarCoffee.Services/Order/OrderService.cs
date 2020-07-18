using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using SolarCoffee.Services.Inventory;
using SolarCoffee.Services.Product;

namespace SolarCoffee.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly SolarDbContext _db;
        private readonly ILogger<OrderService> _logger;
        private readonly IProductService _productService;
        private readonly IInventoryService _inventoryService;

        public OrderService(SolarDbContext db, ILogger<OrderService> loger, IInventoryService inventoryService, IProductService productService)
        {
            _db = db;
            _logger = loger;
            _inventoryService = inventoryService;
        }

        /// <summary>
        ///  Create an open SalesOrder
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public ServiceResponse<bool> GenerateOpenOrder(SalesOrder order)
        {
            _logger.LogInformation("Generatating new order");

            foreach(var item in order.SalesOrderItem){
               
                item.Product=_productService
                        .GetProductById(item.Product.Id);
               
                var inventoryId = _inventoryService
                        .GetByProductId(item.Product.Id).Id;

                _inventoryService
                        .UpdateUnitsAvailable(inventoryId,-item.Quantity);
            }
            try {
                _db.SalesOrders.Add(order);
                _db.SaveChanges();

                return new ServiceResponse<bool> {
                    IsSucces = true,
                    Data = true,
                    Message = "Open order created",
                    Time = DateTime.UtcNow
                };
            } 
            catch (Exception e) {
                return new ServiceResponse<bool> {
                    IsSucces = false,
                    Data = false,
                    Message = e.StackTrace,
                    Time = DateTime.UtcNow
                };
            }
        }

        public List<SalesOrder> GetOrders()
        {
            return _db.SalesOrders
            .Include(salesOrder => salesOrder.Customer)
                .ThenInclude(customer => customer.PrimaryAdress)
            .Include(soa => soa.SalesOrderItem)
                .ThenInclude(item => item.Product)
            .ToList();
        }


        /// <summary>
        ///  Marks an open SalesOrder as paid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServiceResponse<bool> MarkFullfilled(int id)
        {
            var order = _db.SalesOrders.Find(id);
            order.UpdatedOn = DateTime.UtcNow;
            order.IsPaid = true;

            try {
                _db.SalesOrders.Update(order);
                _db.SaveChanges();

                return new ServiceResponse<bool> {
                    IsSucces = true,
                    Data = true,
                    Message = $"Order {order.Id} closed: Invoice paid in full."
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception e) {
                return new ServiceResponse<bool> {
                    IsSucces = false,
                    Data = true,
                    Message = e.StackTrace,
                    Time = DateTime.UtcNow
                };
            }
        }
    }
}