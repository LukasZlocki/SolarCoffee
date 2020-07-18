using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SolarCoffee.Data;

namespace SolarCoffee.Services.Customer
{
    public class CustomerService : ICustomerService
    {

        private readonly SolarDbContext _db;

        public CustomerService(SolarDbContext db)
        {
            _db = db;
        }

        // Returns a list of Customers from the database
        public List<Data.Models.Customer> GetAllCustomers()
        {
            return _db.Customers
                .Include(customer => customer.PrimaryAdress)
                .OrderBy(customer => customer.LastName)
                .ToList();
        }

        // Adds new customer
        public ServiceResponse<Data.Models.Customer> CreateCustomer(Data.Models.Customer customer)
        {
           try {
               _db.Customers.Add(customer);
               _db.SaveChanges();
               return new ServiceResponse<Data.Models.Customer> {
                   IsSucces = true,
                   Message = "New customer created",
                   Time = DateTime.UtcNow,
                   Data = customer
               };

           }
           catch (Exception e){
                   return new ServiceResponse<Data.Models.Customer> {
                   IsSucces = false,
                   Message = e.StackTrace,
                   Time = DateTime.UtcNow,
                   Data = customer
               };
           }
        }

        // Delete customer from database
        public ServiceResponse<bool> DeleteCustomer(int id)
        {
            
            var customer = _db.Customers.Find(id);

            if (customer == null){
                    return new ServiceResponse<bool>{
                        Time=DateTime.UtcNow,
                        IsSucces = false,
                        Message = "Customer to delete not found.",
                        Data = false
                    };
            }
            try {
                _db.Customers.Remove(customer);
                _db.SaveChanges();
                        return new ServiceResponse<bool>{
                        Time = DateTime.UtcNow,
                        IsSucces = true,
                        Message = " Customer removed successfully",
                        Data = true
                        };
            }
            catch (Exception e) {
                        return new ServiceResponse<bool>{
                        Time = DateTime.UtcNow,
                        IsSucces = false,
                        Message = e.StackTrace,
                        Data = false
                    };
            }

            
        }


        // Get a customer records by primary key
        public Data.Models.Customer GetById(int customerId)
        {
            return _db.Customers.Find(customerId);

            // tu mozna wpisac service respond jak nie znajdzie klienta
        }
    }
}