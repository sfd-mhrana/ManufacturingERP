using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManufacturingERP.API.Data;
using ManufacturingERP.API.Models;

namespace ManufacturingERP.API.Controllers
{
    /// <summary>
    /// Suppliers Management API - Manage supplier information
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class SuppliersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SuppliersController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all suppliers
        /// </summary>
        /// <returns>List of suppliers</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Supplier>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers()
        {
            return await _context.Suppliers.Where(s => s.IsActive).ToListAsync();
        }

        /// <summary>
        /// Get supplier by ID
        /// </summary>
        /// <param name="id">Supplier ID</param>
        /// <returns>Supplier details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Supplier), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Supplier>> GetSupplier(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
                return NotFound(new { message = "Supplier not found" });
            return supplier;
        }

        /// <summary>
        /// Create a new supplier
        /// </summary>
        /// <param name="supplier">Supplier data</param>
        /// <returns>Created supplier</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Supplier), StatusCodes.Status201Created)]
        public async Task<ActionResult<Supplier>> CreateSupplier([FromBody] Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSupplier), new { id = supplier.SupplierId }, supplier);
        }

        /// <summary>
        /// Update supplier
        /// </summary>
        /// <param name="id">Supplier ID</param>
        /// <param name="supplier">Updated supplier data</param>
        /// <returns>No content on success</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] Supplier supplier)
        {
            if (id != supplier.SupplierId)
                return BadRequest();

            _context.Entry(supplier).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Delete supplier
        /// </summary>
        /// <param name="id">Supplier ID</param>
        /// <returns>No content on success</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
                return NotFound();

            supplier.IsActive = false;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
