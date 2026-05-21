using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductsApp.Domain.DTOs;

namespace ProductsApp.Domain.Interfaces.Services
{
    public interface IProductService
    {
        Task<ProductDto> CreateAsync(ProductCreateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(ProductUpdateDto dto);

        Task<IEnumerable<ProductDto>> GetBySearchTermAsync(string searchText, int categoryId);
    }
}
