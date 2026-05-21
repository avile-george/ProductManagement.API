using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductsApp.Domain.DTOs;
using ProductsApp.Domain.Interfaces.Services;

namespace ProductsApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        private readonly IProductService _productService = productService;

        /// <summary>
        /// Get all products with pagination
        /// </summary>
        /// <param name="pageNumber">Page number (starting from 1)</param>
        /// <param name="pageSize">Number of products per page (default: 10, max: 100)</param>
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<ProductDto>>> 
            GetAll([FromQuery] string? searchText = null, [FromQuery] int? categoryId = null, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                // Validate pagination parameters
                if (pageNumber < 1)
                    return BadRequest(new { message = "Page number must be greater than or equal to 1" });

                if (pageSize < 1)
                    return BadRequest(new { message = "Page size must be greater than or equal to 1" });

                if (pageSize > 100)
                    return BadRequest(new { message = "Page size cannot exceed 100" });

                // Get all products
                var allProducts = (string.IsNullOrEmpty(searchText) && categoryId == null)
                    ? await _productService.GetAllAsync()
                    : await _productService.GetBySearchTermAsync(searchText, categoryId);

                var productList = allProducts.ToList();
                var totalRecords = productList.Count;

                // Calculate pagination values
                var totalPages = (totalRecords + pageSize - 1) / pageSize;
                if (pageNumber > totalPages && totalRecords > 0)
                    return BadRequest(new { message = $"Page number {pageNumber} exceeds total pages {totalPages}" });

                // Apply pagination
                var paginatedProducts = productList
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // Create paginated response
                var response = new PaginatedResponse<ProductDto>(paginatedProducts, pageNumber, pageSize, totalRecords);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving products", error = ex.Message });
            }
        }

        /// <summary>
        /// Get product by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                    return NotFound(new { message = $"Product with ID {id} not found" });

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the product", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromBody] ProductCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var product = await _productService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = "Invalid product data", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the product", error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing product
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id)
        {
            try
            {
                using var reader = new StreamReader(Request.Body);
                var jsonContent = await reader.ReadToEndAsync();

                if (string.IsNullOrWhiteSpace(jsonContent))
                    return BadRequest(new { message = "Request body cannot be empty" });

                ProductUpdateDto? dto = JsonSerializer.Deserialize<ProductUpdateDto>(jsonContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (dto?.Id != id)
                    return BadRequest(new { message = "Product ID in URL does not match the product ID in the request body" });

                var success = await _productService.UpdateAsync(dto);

                if (!success)
                    return NotFound(new { message = $"Product with ID {id} not found" });

                return NoContent();
            }
            catch (JsonException ex)
            {
                return BadRequest(new { message = "Invalid JSON format", error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = "Invalid product data", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the product", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var success = await _productService.DeleteAsync(id);
                if (!success)
                    return NotFound(new { message = $"Product with ID {id} not found" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the product", error = ex.Message });
            }
        }
    }
}
