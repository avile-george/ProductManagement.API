using System;

namespace ProductsApp.Domain.Models
{
    public class Product : NamedEntity
    {
        public string? Description { get; set; }
        public string? Sku { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
