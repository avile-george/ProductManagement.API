using System.Text.Json.Serialization;

namespace ProductsApp.Domain.DTOs
{
    public record CategoryDto
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }
        [JsonPropertyName("name")]
        public string? Name { get; init; }
        [JsonPropertyName("description")]
        public string? Description { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public int? ParentCategoryId { get; set; }
        public List<CategoryDto>? SubCategories { get; set; } = [];
    }
}
