## Namespace convention

All source files must have namespaces that reflect their project and folder location. For example:

- `ProductsApp.API\Controllers\ProductsController.cs` -> `namespace ProductsApp.API.Controllers`
- `ProductsApp.Shared\Models\BaseEntity.cs` -> `namespace ProductsApp.Shared.Models`
- `ProductsApp.Data\Repositories\ProductRepository.cs` -> `namespace ProductsApp.Data.Repositories`

This file enforces these conventions and will be used when updating files.

## Services
Services in `ProductsApp.Application` should implement interfaces defined in `ProductsApp.Domain.Interfaces.Services` and use repositories from `ProductsApp.Domain.Interfaces.Repositories`. DTOs live in `ProductsApp.Domain.DTOs` and domain models in `ProductsApp.Domain.Models`. AutoMapper is available in the application project.

## Service interfaces
Services in `ProductsApp.Domain.Interfaces.Services` should use DTOs in `ProductsApp.Domain.DTOs` for API contracts and domain models in `ProductsApp.Domain.Models` for business logic. Example interface:

```csharp
namespace ProductsApp.Domain.Interfaces.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<ProductDto> CreateAsync(ProductCreateDto dto);
        Task<bool> UpdateAsync(ProductUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}