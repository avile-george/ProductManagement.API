namespace ProductsApp.Domain.DTOs
{
    public record CategoryCreateDto
    {
        public string? Name { get; init; }
        public string? Description { get; init; }
    }
}
