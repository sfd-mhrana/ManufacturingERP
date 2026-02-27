using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManufacturingERP.API.Data;
using ManufacturingERP.API.DTOs;

namespace ManufacturingERP.API.Controllers
{
    /// <summary>
    /// Inventory Management API - Track stock levels and manage warehouse inventory
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class InventoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InventoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all inventory items with optional filtering
        /// </summary>
        /// <param name="warehouseId">Filter by warehouse</param>
        /// <param name="lowStockOnly">Show only low stock items</param>
        /// <returns>List of inventory items</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InventoryDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<InventoryDto>>> GetInventory(
            [FromQuery] int? warehouseId = null,
            [FromQuery] bool lowStockOnly = false)
        {
            var query = _context.Inventories
                .Include(i => i.Product)
                .Include(i => i.Warehouse)
                .AsQueryable();

            if (warehouseId.HasValue)
                query = query.Where(i => i.WarehouseId == warehouseId.Value);

            var inventoryList = await query.Select(i => new InventoryDto
            {
                InventoryId = i.InventoryId,
                ProductId = i.ProductId,
                ProductName = i.Product != null ? i.Product.ProductName : "",
                SKU = i.Product != null ? i.Product.SKU : "",
                WarehouseId = i.WarehouseId,
                WarehouseName = i.Warehouse != null ? i.Warehouse.WarehouseName : "",
                QuantityOnHand = i.QuantityOnHand,
                QuantityReserved = i.QuantityReserved,
                QuantityAvailable = i.QuantityOnHand - i.QuantityReserved,
                UnitCost = i.UnitCost,
                TotalValue = i.QuantityOnHand * i.UnitCost,
                LastStockUpdate = i.LastStockUpdate,
                IsLowStock = i.Product != null && i.QuantityOnHand <= i.Product.ReorderLevel
            }).ToListAsync();

            if (lowStockOnly)
                inventoryList = inventoryList.Where(i => i.IsLowStock).ToList();

            return Ok(inventoryList);
        }

        /// <summary>
        /// Get inventory by ID
        /// </summary>
        /// <param name="id">Inventory ID</param>
        /// <returns>Inventory details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(InventoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<InventoryDto>> GetInventoryById(int id)
        {
            var inventory = await _context.Inventories
                .Include(i => i.Product)
                .Include(i => i.Warehouse)
                .FirstOrDefaultAsync(i => i.InventoryId == id);

            if (inventory == null)
                return NotFound(new { message = "Inventory item not found" });

            return Ok(new InventoryDto
            {
                InventoryId = inventory.InventoryId,
                ProductId = inventory.ProductId,
                ProductName = inventory.Product?.ProductName ?? "",
                SKU = inventory.Product?.SKU ?? "",
                WarehouseId = inventory.WarehouseId,
                WarehouseName = inventory.Warehouse?.WarehouseName ?? "",
                QuantityOnHand = inventory.QuantityOnHand,
                QuantityReserved = inventory.QuantityReserved,
                QuantityAvailable = inventory.QuantityOnHand - inventory.QuantityReserved,
                UnitCost = inventory.UnitCost,
                TotalValue = inventory.QuantityOnHand * inventory.UnitCost,
                LastStockUpdate = inventory.LastStockUpdate,
                IsLowStock = inventory.Product != null && inventory.QuantityOnHand <= inventory.Product.ReorderLevel
            });
        }

        /// <summary>
        /// Update inventory quantity
        /// </summary>
        /// <param name="id">Inventory ID</param>
        /// <param name="dto">Updated inventory data</param>
        /// <returns>No content on success</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateInventory(int id, [FromBody] UpdateInventoryDto dto)
        {
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null)
                return NotFound(new { message = "Inventory item not found" });

            inventory.QuantityOnHand = dto.QuantityOnHand;
            inventory.QuantityReserved = dto.QuantityReserved;
            inventory.UnitCost = dto.UnitCost;
            inventory.LastStockUpdate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Get low stock alerts
        /// </summary>
        /// <returns>List of low stock items</returns>
        [HttpGet("low-stock")]
        [ProducesResponseType(typeof(IEnumerable<InventoryDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<InventoryDto>>> GetLowStockItems()
        {
            var lowStockItems = await _context.Inventories
                .Include(i => i.Product)
                .Include(i => i.Warehouse)
                .Where(i => i.Product != null && i.QuantityOnHand <= i.Product.ReorderLevel)
                .Select(i => new InventoryDto
                {
                    InventoryId = i.InventoryId,
                    ProductId = i.ProductId,
                    ProductName = i.Product != null ? i.Product.ProductName : "",
                    SKU = i.Product != null ? i.Product.SKU : "",
                    WarehouseId = i.WarehouseId,
                    WarehouseName = i.Warehouse != null ? i.Warehouse.WarehouseName : "",
                    QuantityOnHand = i.QuantityOnHand,
                    QuantityReserved = i.QuantityReserved,
                    QuantityAvailable = i.QuantityOnHand - i.QuantityReserved,
                    UnitCost = i.UnitCost,
                    TotalValue = i.QuantityOnHand * i.UnitCost,
                    LastStockUpdate = i.LastStockUpdate,
                    IsLowStock = true
                }).ToListAsync();

            return Ok(lowStockItems);
        }

        /// <summary>
        /// Transfer stock between warehouses
        /// </summary>
        /// <param name="dto">Stock transfer details</param>
        /// <returns>Success message</returns>
        [HttpPost("transfer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> TransferStock([FromBody] StockTransferDto dto)
        {
            var sourceInventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.ProductId == dto.ProductId && i.WarehouseId == dto.FromWarehouseId);

            if (sourceInventory == null)
                return BadRequest(new { message = "Source inventory not found" });

            if (sourceInventory.QuantityAvailable < dto.Quantity)
                return BadRequest(new { message = "Insufficient stock for transfer" });

            var destInventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.ProductId == dto.ProductId && i.WarehouseId == dto.ToWarehouseId);

            sourceInventory.QuantityOnHand -= dto.Quantity;
            sourceInventory.LastStockUpdate = DateTime.UtcNow;

            if (destInventory != null)
            {
                destInventory.QuantityOnHand += dto.Quantity;
                destInventory.LastStockUpdate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Stock transfer completed successfully" });
        }
    }
}
