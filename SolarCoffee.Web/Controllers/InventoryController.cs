using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffee.Services.Inventory;
using SolarCoffee.Web.ViewModels;

namespace SolarCoffee.Web.Controllers
{
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(IInventoryService inventoryService, ILogger<InventoryController> logger)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }

    [HttpGet("/api/inventory")]
    public ActionResult GetCurrentInventory(){
        _logger.LogInformation("Getting all inventory");
        var inventory = _inventoryService.GetCurrentInventory()
            .Select(pi => new ProductInventoryModel {
                Id = pi.Id,
                QuantityOnHand = pi.QuantityOnHand,
                IdealQuantity = pi.IdealQuantity,
            })
            .OrderBy(inv => inv.Product.Name)
            .ToList();

        return Ok(inventory);
    }


    [HttpPatch("/api/inventory")]
        public ActionResult Updateinventory([FromBody] ShipmentModel shipment) {
            _logger.LogInformation(
                "Update inventory " + 
                    $"for {shipment.ProductId} - " +
                    $"Adjustment : {shipment.Adjustment}"
                    );
            var inventory = _inventoryService.GetCurrentInventory()
                .Select(pi => new ProductInventoryModel {
                    Id = pi.Id,
                    QuantityOnHand = pi.QuantityOnHand,
                    IdealQuantity = pi.IdealQuantity,
                })
                .OrderBy(inv => inv.Product.Name)
                .ToList();

            return Ok(inventory);
        }


    }
}