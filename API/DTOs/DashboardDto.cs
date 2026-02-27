namespace ManufacturingERP.API.DTOs
{
    public class DashboardDto
    {
        public int TotalProducts { get; set; }
        public int TotalSuppliers { get; set; }
        public int TotalWarehouses { get; set; }
        public int LowStockItems { get; set; }
        public int PendingOrders { get; set; }
        public decimal TotalInventoryValue { get; set; }
        public List<CategoryStockDto> StockByCategory { get; set; } = new();
        public List<RecentActivityDto> RecentActivities { get; set; } = new();
        public List<TopProductDto> TopProducts { get; set; } = new();
        public List<MonthlyOrderDto> MonthlyOrders { get; set; } = new();
    }

    public class CategoryStockDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public int TotalQuantity { get; set; }
        public decimal TotalValue { get; set; }
    }

    public class RecentActivityDto
    {
        public string ActivityType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }

    public class TopProductDto
    {
        public string ProductName { get; set; } = string.Empty;
        public int QuantityInStock { get; set; }
        public decimal TotalValue { get; set; }
    }

    public class MonthlyOrderDto
    {
        public string Month { get; set; } = string.Empty;
        public int OrderCount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
