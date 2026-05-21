namespace ProductsApp.Domain.DTOs
{
    public record ProductCreateDto
    {
        public string? Name { get; init; }
        public string? Description { get; init; }
        public decimal? Price { get; init; }
        public int? CategoryId { get; init; }
        public int? Quantity { get; set; }
        public string? Sku { get; set; }
    }
}
