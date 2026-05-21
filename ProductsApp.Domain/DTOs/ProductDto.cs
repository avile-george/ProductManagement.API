namespace ProductsApp.Domain.DTOs
{
    public record ProductDto : IComparable<ProductDto>
    {
        public int Id { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        public int? Quantity { get; set; }
        public string? Sku { get; set; }
        public decimal Price { get; init; }
        public int? CategoryId { get; init; }

        public int CompareTo(ProductDto? other)
        {
           if (other is null) return 1;

           return string.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }
    }
}
