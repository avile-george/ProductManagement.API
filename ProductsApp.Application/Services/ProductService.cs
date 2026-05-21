using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ProductsApp.Application.Helpers;
using ProductsApp.Domain.DTOs;
using ProductsApp.Domain.Interfaces.Repositories;
using ProductsApp.Domain.Interfaces.SearchEngines;
using ProductsApp.Domain.Interfaces.Services;
using ProductsApp.Domain.Models;

namespace ProductsApp.Application.Services
{
    public class ProductService(
            IProductRepository products,
            ICategoryRepository categories,
            IMapper mapper,
            IProductSearchEngine searchEngine,
            ICacheHelper cacheHelper) : IProductService
    {
        private readonly IProductRepository _products = products;
        private readonly ICategoryRepository _categories = categories;
        private readonly IMapper _mapper = mapper;
        private readonly IProductSearchEngine _searchEngine = searchEngine;
        private readonly ICacheHelper _cacheHelper = cacheHelper;
        private const string CACHEKEY = "ProductsCacheKey";
        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var models = _cacheHelper.Get<IEnumerable<Product>>(CACHEKEY);

            if (models?.Any() == true)
                goto End;

            models = await _products.GetAllAsync();

            if (models?.Any() == true)
                _cacheHelper.Set(CACHEKEY, models);

            End:
            return _mapper.Map<IEnumerable<ProductDto>>(models);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var model = await _products.GetByIdAsync(id);
            if (model is null) return null;
            return _mapper.Map<ProductDto>(model);
        }

        public async Task<IEnumerable<ProductDto>> GetBySearchTermAsync(string searchText, int? categoryId)
        {
            var matches = (categoryId > 0) ?
                await _products.SearchAsync(p => p.CategoryId == categoryId) :
                await _products.GetAllAsync();

            if (matches?.Any() == true && !string.IsNullOrWhiteSpace(searchText))
                matches = _searchEngine.GetByFuzzyMatch(matches, searchText);

            return _mapper.Map<IEnumerable<ProductDto>>(matches);
        }

        public async Task<ProductDto> CreateAsync(ProductCreateDto dto)
        {
            if (dto is { Name: null or "" })
                throw new ArgumentException("Product name cannot be empty", nameof(dto.Name));

            var model = _mapper.Map<Product>(dto);

            // validate category exists
            _ = await _categories.GetByIdAsync(model.CategoryId) ?? throw new KeyNotFoundException("Category not found");
            var created = await _products.CreateAsync(model) ?? throw new Exception("Failed to create product");
            _cacheHelper.Clear(CACHEKEY); // invalidate cache

            return _mapper.Map<ProductDto>(created);
        }

        public async Task<bool> UpdateAsync(ProductUpdateDto dto)
        {
            if (dto is { Name: null or "" })
                throw new ArgumentException("Product name cannot be empty", nameof(dto.Name));

            var exists = await _products.GetByIdAsync(dto.Id);
            if (exists is null) return false;

            var model = _mapper.Map<Product>(dto);
            var cat = await _categories.GetByIdAsync(model.CategoryId);
            if (cat is null) return false;

            var success = await _products.UpdateAsync(model);

            if (success)
                _cacheHelper.Clear(CACHEKEY); // invalidate cache

            return success;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var success = await _products.DeleteAsync(id);
            if (success)
                _cacheHelper.Clear(CACHEKEY); // invalidate cache

            return success;
        }
    }
}
