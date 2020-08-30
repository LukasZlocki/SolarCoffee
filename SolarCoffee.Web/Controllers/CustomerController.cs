using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data.Models;
using SolarCoffee.Services.Customer;
using SolarCoffee.Services.Order;
using SolarCoffee.Web.Serialization;
using SolarCoffee.Web.ViewModels;

namespace SolarCoffee.Web.Controllers
{
    [ApiController]
    public class CustomerController : ControllerBase {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService; 

        public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService, IOrderService orderService)
        {
            _logger = logger;
            _customerService = customerService;
            _orderService = orderService;
        }
        
        [HttpPost("api/customer")]
        public ActionResult CreateCustomer([FromBody] CustomerModel customer){
            _logger.LogInformation("Creating a new customer");
            customer.CreatedOn = DateTime.UtcNow;
            customer.UpdatedOn = DateTime.UtcNow;
            var customerData = CustomerMapper.SerializeCustomer(customer);
            var newCustomer = _customerService.CreateCustomer(customerData);
            return Ok(newCustomer);
        }

        [HttpGet("/api/customer")]
        public ActionResult GetCustomer() {
            _logger.LogInformation("Getting customers");
            var customers = _customerService.GetAllCustomers();

            var customerModels = customers
            .Select(customer => new CustomerModel {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PrimaryAddress = CustomerMapper
                    .MapCustomerAddress(customer.PrimaryAdress),
                CreatedOn = customer.CreatedOn,
                UpdatedOn = customer.UpdatedOn
            })
            .OrderByDescending(customer => customer.CreatedOn)
            .ToList();

            return Ok(customerModels);
        }

    [HttpDelete("/api/customer/{id}")]
    public ActionResult DeleteCustomer(int id) {
        _logger.LogInformation("Deleting a customer");
        var response = _customerService
            .DeleteCustomer(id);
        return Ok(response);
    }

    [HttpGet("/api/order")]
    public ActionResult GetOrders() {
        _logger.LogInformation("Get all orders");
        var orders = _orderService.GetOrders();
        var orderModels = OrderMapper.SerializeOrdersToViewModel(orders);
        return Ok(orderModels);
    }

    [HttpGet("/api/order/complete/{id}")]
        public ActionResult MarkOrderComplete(int id) {
            _logger.LogInformation($"Marking order {id} complete.");
            _orderService.MarkFullfilled(id);
            return Ok();
    }


    }
}