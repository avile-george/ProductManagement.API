using System.Xml.Linq;
using AutoMapper;
using ProductsApp.Domain.DTOs;
using ProductsApp.Domain.Interfaces.Repositories;
using ProductsApp.Domain.Interfaces.Services;
using ProductsApp.Domain.Models;

namespace ProductsApp.Application.Services
{

    public class CategoryService(
        ICategoryRepository categories,
        IProductRepository products,
        IMapper mapper) : ICategoryService
    {
        private readonly ICategoryRepository _categories = categories;
        private readonly IProductRepository _products = products;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var models = await _categories.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(models);
        }

        public async Task<IEnumerable<CategoryDto>> GetHierarchy()
        {
            var flatList = await GetAllAsync() ?? [];
            var roots = new List<CategoryDto>();

            if (!flatList.Any()) return roots;

            var lookup = flatList.ToDictionary(x => x.Id, x => x);

            foreach (var item in lookup.Values)
            {
                if (item.ParentCategoryId.HasValue && lookup.TryGetValue(item.ParentCategoryId.Value, out var parent))
                {
                    // if item has a parent, add it to the parent's SubCategories
                    parent.SubCategories.Add(item);
                }
                else
                {
                    // if item has no parent, it's a root category
                    roots.Add(item);
                }
            }

            return roots;
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var model = await _categories.GetByIdAsync(id);
            if (model is null) return null;
            return _mapper.Map<CategoryDto>(model);
        }

        public async Task<CategoryDto> CreateAsync(CategoryCreateDto dto)
        {
            if (dto is { Name: null or "" })
                throw new ArgumentException("Category name cannot be empty", nameof(dto.Name));

            var model = _mapper.Map<Category>(dto);
            var created = await _categories.CreateAsync(model);
            return _mapper.Map<CategoryDto>(created);
        }

        public async Task<bool> UpdateAsync(CategoryUpdateDto dto)
        {
            if (dto is { Id: null })
                throw new ArgumentException("Category identifier cannot be empty", nameof(dto.Id));

            if (dto is { Name: null or "" })
                throw new ArgumentException("Category name cannot be empty", nameof(dto.Name));

            var exists = await _categories.GetByIdAsync(dto.Id.Value);
            if (exists is null) return false;

            var model = _mapper.Map<Category>(dto);
            return await _categories.UpdateAsync(model);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var products = await _products.SearchAsync(p => p.CategoryId == id);
            if (products.Any())
            {
                return false;
            }

            return await _categories.DeleteAsync(id);
        }
    }
}
