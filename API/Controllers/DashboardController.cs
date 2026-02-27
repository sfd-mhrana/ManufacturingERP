using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManufacturingERP.API.Data;
using ManufacturingERP.API.DTOs;

namespace ManufacturingERP.API.Controllers
{
    /// <summary>
    /// Dashboard API - Analytics and KPIs for the ERP system
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get dashboard overview with all KPIs
        /// </summary>
        /// <returns>Dashboard data with analytics</returns>
        [HttpGet]
        [ProducesResponseType(typeof(DashboardDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<DashboardDto>> GetDashboard()
        {
            var totalProducts = await _context.Products.CountAsync(p => p.IsActive);
            var totalSuppliers = await _context.Suppliers.CountAsync(s => s.IsActive);
            var totalWarehouses = await _context.Warehouses.CountAsync(w => w.IsActive);

            var inventoryData = await _context.Inventories
                .Include(i => i.Product)
                .ToListAsync();

            var lowStockItems = inventoryData.Count(i => i.Product != null && i.QuantityOnHand <= i.Product.ReorderLevel);
            var totalInventoryValue = inventoryData.Sum(i => i.QuantityOnHand * i.UnitCost);

            var pendingOrders = await _context.PurchaseOrders
                .CountAsync(po => po.Status == "Pending" || po.Status == "Approved");

            // Stock by Category
            var stockByCategory = await _context.Inventories
                .Include(i => i.Product)
                .ThenInclude(p => p!.Category)
                .GroupBy(i => i.Product!.Category!.CategoryName)
                .Select(g => new CategoryStockDto
                {
                    CategoryName = g.Key,
                    TotalQuantity = g.Sum(i => i.QuantityOnHand),
                    TotalValue = g.Sum(i => i.QuantityOnHand * i.UnitCost)
                }).ToListAsync();

            // Top Products by Value
            var topProducts = await _context.Inventories
                .Include(i => i.Product)
                .OrderByDescending(i => i.QuantityOnHand * i.UnitCost)
                .Take(5)
                .Select(i => new TopProductDto
                {
                    ProductName = i.Product != null ? i.Product.ProductName : "",
                    QuantityInStock = i.QuantityOnHand,
                    TotalValue = i.QuantityOnHand * i.UnitCost
                }).ToListAsync();

            // Recent Activities (simulated)
            var recentActivities = new List<RecentActivityDto>
            {
                new RecentActivityDto { ActivityType = "Stock Update", Description = "Steel Sheet 4x8 - Added 100 units", Timestamp = DateTime.UtcNow.AddHours(-1) },
                new RecentActivityDto { ActivityType = "Purchase Order", Description = "PO-2025-001 - Submitted to ABC Manufacturing Ltd", Timestamp = DateTime.UtcNow.AddHours(-3) },
                new RecentActivityDto { ActivityType = "Low Stock Alert", Description = "Electric Motor 5HP - Below reorder level", Timestamp = DateTime.UtcNow.AddHours(-5) },
                new RecentActivityDto { ActivityType = "Inventory Count", Description = "Main Warehouse - Cycle count completed", Timestamp = DateTime.UtcNow.AddDays(-1) },
                new RecentActivityDto { ActivityType = "New Product", Description = "Control Panel Unit - Added to catalog", Timestamp = DateTime.UtcNow.AddDays(-2) }
            };

            // Monthly Orders (simulated data)
            var monthlyOrders = new List<MonthlyOrderDto>
            {
                new MonthlyOrderDto { Month = "Jan", OrderCount = 45, TotalAmount = 125000 },
                new MonthlyOrderDto { Month = "Feb", OrderCount = 52, TotalAmount = 148000 },
                new MonthlyOrderDto { Month = "Mar", OrderCount = 48, TotalAmount = 132000 },
                new MonthlyOrderDto { Month = "Apr", OrderCount = 61, TotalAmount = 175000 },
                new MonthlyOrderDto { Month = "May", OrderCount = 55, TotalAmount = 156000 },
                new MonthlyOrderDto { Month = "Jun", OrderCount = 67, TotalAmount = 198000 }
            };

            return Ok(new DashboardDto
            {
                TotalProducts = totalProducts,
                TotalSuppliers = totalSuppliers,
                TotalWarehouses = totalWarehouses,
                LowStockItems = lowStockItems,
                PendingOrders = pendingOrders,
                TotalInventoryValue = totalInventoryValue,
                StockByCategory = stockByCategory,
                TopProducts = topProducts,
                RecentActivities = recentActivities,
                MonthlyOrders = monthlyOrders
            });
        }

        /// <summary>
        /// Get inventory summary statistics
        /// </summary>
        /// <returns>Inventory statistics</returns>
        [HttpGet("inventory-stats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInventoryStats()
        {
            var stats = await _context.Inventories
                .Include(i => i.Product)
                .GroupBy(i => 1)
                .Select(g => new
                {
                    TotalItems = g.Count(),
                    TotalQuantity = g.Sum(i => i.QuantityOnHand),
                    TotalValue = g.Sum(i => i.QuantityOnHand * i.UnitCost),
                    AverageValue = g.Average(i => i.QuantityOnHand * i.UnitCost),
                    LowStockCount = g.Count(i => i.Product != null && i.QuantityOnHand <= i.Product.ReorderLevel)
                }).FirstOrDefaultAsync();

            return Ok(stats);
        }

        /// <summary>
        /// Get supplier summary
        /// </summary>
        /// <returns>Supplier statistics</returns>
        [HttpGet("supplier-stats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSupplierStats()
        {
            var suppliers = await _context.Suppliers
                .Include(s => s.Products)
                .Where(s => s.IsActive)
                .Select(s => new
                {
                    s.CompanyName,
                    ProductCount = s.Products.Count,
                    TotalProductValue = s.Products.Sum(p => p.UnitPrice)
                })
                .OrderByDescending(s => s.ProductCount)
                .Take(5)
                .ToListAsync();

            return Ok(suppliers);
        }
    }
}
