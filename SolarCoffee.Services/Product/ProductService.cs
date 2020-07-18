using System;
using System.Collections.Generic;
using System.Linq;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Services.Product
{
    public class ProductService : IProductService
    {

        private readonly SolarDbContext _db;

        public ProductService(SolarDbContext db)
        {
            _db = db;
        }

        // Archive product by setting boolean IsArchived to true
        public ServiceResponse<Data.Models.Product> ArchiveProduct(int id)
        {
            try
            {
                var product = _db.Products.Find(id);
                product.IsArchive = true;
                _db.SaveChanges();
                return new ServiceResponse<Data.Models.Product>
                {
                    Data = product,
                    Time = DateTime.UtcNow,
                    Message = "Product archived",
                    IsSucces = true
                };


            }
            catch (Exception e)
            {
                return new ServiceResponse<Data.Models.Product>
                {
                    Data = null,
                    Time = DateTime.UtcNow,
                    Message = e.StackTrace,
                    IsSucces = false
                };
            }
        }

    
 
        /// Adds a new product to database
        public ServiceResponse<Data.Models.Product> CreateProduct(Data.Models.Product product)
        {
           try
           {
                _db.Products.Add(product);

            var newInventory = new ProductInventory 
            {
                Product = product,
                QuantityOnHand = 0,
                IdealQuantity = 10
            };
            _db.ProductInventories.Add(newInventory);
            _db.SaveChanges();

            return new ServiceResponse<Data.Models.Product>
            {
                Data = product,
                Time = DateTime.UtcNow,
                Message = "Saved new production",
                IsSucces = true
            };
           }
           catch (Exception e)
           {
               return new ServiceResponse<Data.Models.Product>
               {
                Data = product,
                Time = DateTime.UtcNow,
                Message = e.StackTrace,
                IsSucces = false
               };
           }
        }

        // Retrieves a Product from database by primary key
        public List<Data.Models.Product> GetAllProducts()
        {
            return _db.Products.ToList();
        }

        public Data.Models.Product GetProductById(int id)
        {
            return _db.Products.Find(id);
        }
    }
}