using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManufacturingERP.API.Data;
using ManufacturingERP.API.Models;

namespace ManufacturingERP.API.Controllers
{
    /// <summary>
    /// Warehouses Management API - Manage warehouse locations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class WarehousesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WarehousesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all warehouses
        /// </summary>
        /// <returns>List of warehouses</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Warehouse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Warehouse>>> GetWarehouses()
        {
            return await _context.Warehouses.Where(w => w.IsActive).ToListAsync();
        }

        /// <summary>
        /// Get warehouse by ID
        /// </summary>
        /// <param name="id">Warehouse ID</param>
        /// <returns>Warehouse details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Warehouse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Warehouse>> GetWarehouse(int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse == null)
                return NotFound(new { message = "Warehouse not found" });
            return warehouse;
        }

        /// <summary>
        /// Create a new warehouse
        /// </summary>
        /// <param name="warehouse">Warehouse data</param>
        /// <returns>Created warehouse</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Warehouse), StatusCodes.Status201Created)]
        public async Task<ActionResult<Warehouse>> CreateWarehouse([FromBody] Warehouse warehouse)
        {
            _context.Warehouses.Add(warehouse);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetWarehouse), new { id = warehouse.WarehouseId }, warehouse);
        }

        /// <summary>
        /// Update warehouse
        /// </summary>
        /// <param name="id">Warehouse ID</param>
        /// <param name="warehouse">Updated warehouse data</param>
        /// <returns>No content on success</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateWarehouse(int id, [FromBody] Warehouse warehouse)
        {
            if (id != warehouse.WarehouseId)
                return BadRequest();

            _context.Entry(warehouse).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Delete warehouse
        /// </summary>
        /// <param name="id">Warehouse ID</param>
        /// <returns>No content on success</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteWarehouse(int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse == null)
                return NotFound();

            warehouse.IsActive = false;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
