namespace ProductsApp.Domain.Models
{
    public class Category : NamedEntity
    {
        public string? Description { get; set; }
        public int ParentCategoryId { get; set; }
    }
}
