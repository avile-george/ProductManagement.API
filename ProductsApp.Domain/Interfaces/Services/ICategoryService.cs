using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductsApp.Domain.DTOs;

namespace ProductsApp.Domain.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<CategoryDto> CreateAsync(CategoryCreateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<IEnumerable<CategoryDto>> GetHierarchy();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(CategoryUpdateDto dto);
    }
}
