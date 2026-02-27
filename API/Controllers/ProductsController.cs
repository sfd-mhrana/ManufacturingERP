using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManufacturingERP.API.Data;
using ManufacturingERP.API.DTOs;
using ManufacturingERP.API.Models;

namespace ManufacturingERP.API.Controllers
{
    /// <summary>
    /// Products Management API - Handles CRUD operations for products
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all products with optional filtering
        /// </summary>
        /// <param name="categoryId">Filter by category</param>
        /// <param name="supplierId">Filter by supplier</param>
        /// <param name="isActive">Filter by active status</param>
        /// <returns>List of products</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts(
            [FromQuery] int? categoryId = null,
            [FromQuery] int? supplierId = null,
            [FromQuery] bool? isActive = null)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            if (supplierId.HasValue)
                query = query.Where(p => p.SupplierId == supplierId.Value);

            if (isActive.HasValue)
                query = query.Where(p => p.IsActive == isActive.Value);

            var products = await query.Select(p => new ProductDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                SKU = p.SKU,
                Description = p.Description,
                UnitPrice = p.UnitPrice,
                ReorderLevel = p.ReorderLevel,
                CategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.CategoryName : "",
                SupplierId = p.SupplierId,
                SupplierName = p.Supplier != null ? p.Supplier.CompanyName : "",
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt
            }).ToListAsync();

            return Ok(products);
        }

        /// <summary>
        /// Get a specific product by ID
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Product details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
                return NotFound(new { message = "Product not found" });

            return Ok(new ProductDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                SKU = product.SKU,
                Description = product.Description,
                UnitPrice = product.UnitPrice,
                ReorderLevel = product.ReorderLevel,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.CategoryName ?? "",
                SupplierId = product.SupplierId,
                SupplierName = product.Supplier?.CompanyName ?? "",
                IsActive = product.IsActive,
                CreatedAt = product.CreatedAt
            });
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        /// <param name="dto">Product creation data</param>
        /// <returns>Created product</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto dto)
        {
            var product = new Product
            {
                ProductName = dto.ProductName,
                SKU = dto.SKU,
                Description = dto.Description,
                UnitPrice = dto.UnitPrice,
                ReorderLevel = dto.ReorderLevel,
                CategoryId = dto.CategoryId,
                SupplierId = dto.SupplierId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }

        /// <summary>
        /// Update an existing product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="dto">Updated product data</param>
        /// <returns>No content on success</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound(new { message = "Product not found" });

            product.ProductName = dto.ProductName;
            product.SKU = dto.SKU;
            product.Description = dto.Description;
            product.UnitPrice = dto.UnitPrice;
            product.ReorderLevel = dto.ReorderLevel;
            product.CategoryId = dto.CategoryId;
            product.SupplierId = dto.SupplierId;
            product.IsActive = dto.IsActive;
            product.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>No content on success</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound(new { message = "Product not found" });

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
