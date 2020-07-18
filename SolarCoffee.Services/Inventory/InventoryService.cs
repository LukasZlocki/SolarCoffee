using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Services.Inventory
{
    public class InventoryService : IInventoryService
    {

        private readonly SolarDbContext _db;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(SolarDbContext db, ILogger<InventoryService> logger)
        {
            _db = db;
            _logger = logger;
        }

        // Returns all current inventory from the data 
        public List<ProductInventory> GetCurrentInventory()
        {
            return _db.ProductInventories
            .Include(p => p.Product)
            .Where(p => !p.Product.IsArchive)
            .ToList();
        }

        // Get ProductInventory instance by Product ID
        public ProductInventory GetByProductId(int productID)
        {            
            return _db.ProductInventories
                .Include(p => p.Product)
                .FirstOrDefault(p => p.Product.Id == productID);                
        }


        public void CreateSnapshot(ProductInventory inventory) {
            var snapshot = new ProductInventorySnapshot {
                SnapshotTime = DateTime.UtcNow,
                Product = inventory.Product,
                QuantityOnHand = inventory.QuantityOnHand
            };

            // z automatu doda do dobrej tabeli poniewaz zna typ
            _db.Add(snapshot);
            _db.SaveChanges(); // komentarz
        }
        
            
        public List<ProductInventorySnapshot> GetSnapshotHistory()
        {
            var earliest = DateTime.UtcNow - TimeSpan.FromHours(6);

            return _db.ProductInventorySnapshots
                .Include(snap => snap.Product)
                .Where(snap => snap.SnapshotTime > earliest && !snap.Product.IsArchive)
                .ToList();
        }

        // Updates numberof units available of the provided product id and adjust quanity by adjustment value
        public ServiceResponse<ProductInventory> UpdateUnitsAvailable(int id, int adjustment)
        {
            try {
                var inventory = _db.ProductInventories.
                    Include(inv => inv.Product).
                    First(inventory => inventory.Product.Id == id );

                inventory.QuantityOnHand += adjustment;

                try { 
                    CreateSnapshot(inventory);
                }
                catch (Exception e) {
                    _logger.LogError("Error creating inventory snapshot");
                    _logger.LogError(e.StackTrace);
                }

                _db.SaveChanges();

                return new ServiceResponse<ProductInventory> {
                    IsSucces = true,
                    Data = inventory,
                    Message = $"Product {id} inventory created",
                    Time = DateTime.UtcNow
                };

            }
            catch {
                return new ServiceResponse<ProductInventory> {
                    IsSucces = false,
                    Data = null,
                    Message = $"Error during updating inventory of product",
                    Time = DateTime.UtcNow
                };
            }
        }
    }
}