using System;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using SolarCoffee.Web.ViewModels;

namespace SolarCoffee.Web.Serialization
{
    public static class CustomerMapper
    {
        
        // Serializes a Customer data model into a CustomerModel view model
        public static CustomerModel SerializeCustomer (Customer customer) {
            return new CustomerModel {
                Id = customer.Id,
                CreatedOn = customer.CreatedOn,
                UpdatedOn = customer.UpdatedOn,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PrimaryAddress = MapCustomerAddress(customer.PrimaryAdress),
            };       
        }


         // Serializes a CustomerModel view model into a Customer data model
        public static Customer SerializeCustomer (CustomerModel customer) {
            return new Customer {
                CreatedOn = customer.CreatedOn,
                UpdatedOn = customer.UpdatedOn,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PrimaryAdress = MapCustomerAddress(customer.PrimaryAddress),
            };       
        }

       // Maps a CustomerAddress data model to a CustomerAdressModel view model
        public static CustomerAddressModel MapCustomerAddress(CustomerAddress address) {
            return new CustomerAddressModel {
                Id = address.Id,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                City = address.City,
                State = address.State,
                County = address.County,
                PostalCode = address.PostalCode,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
            };
        }


        // Maps a CustomerAddressModel view model to a CustomerAdress data model
        public static CustomerAddress MapCustomerAddress(CustomerAddressModel address) {
            return new CustomerAddress {
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                City = address.City,
                State = address.State,
                County = address.County,
                PostalCode = address.PostalCode,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
            };
        }

    }
}