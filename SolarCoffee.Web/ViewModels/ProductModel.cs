using System;

namespace SolarCoffee.Web.ViewModels
{
    // Product entity DTO
    public class ProductModel
    {
        public int Id { get; set; }
        public DateTime CreatedOn  { get; set; }
        public DateTime UpdatedOn { get; set; }       
        public string Name { get; set; }
        public string Description { get; set; }      
        public decimal Price { get; set; }
        public bool IsTaxable { get; set; }
        public bool IsArchive { get; set; }
    }
}