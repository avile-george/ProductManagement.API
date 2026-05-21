namespace ProductsApp.Domain.DTOs
{
    public record CategoryUpdateDto
    {
        public int? Id { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
    }
}
