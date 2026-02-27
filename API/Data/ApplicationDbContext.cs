using Microsoft.EntityFrameworkCore;
using ManufacturingERP.API.Models;

namespace ManufacturingERP.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Supplier> Suppliers { get; set; } = null!;
        public DbSet<Inventory> Inventories { get; set; } = null!;
        public DbSet<Warehouse> Warehouses { get; set; } = null!;
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; } = null!;
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Warehouse)
                .WithMany(w => w.Inventories)
                .HasForeignKey(i => i.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(po => po.Supplier)
                .WithMany(s => s.PurchaseOrders)
                .HasForeignKey(po => po.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PurchaseOrderItem>()
                .HasOne(poi => poi.PurchaseOrder)
                .WithMany(po => po.Items)
                .HasForeignKey(poi => poi.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchaseOrderItem>()
                .HasOne(poi => poi.Product)
                .WithMany()
                .HasForeignKey(poi => poi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "Raw Materials", Description = "Basic materials for manufacturing" },
                new Category { CategoryId = 2, CategoryName = "Components", Description = "Assembled parts and components" },
                new Category { CategoryId = 3, CategoryName = "Finished Goods", Description = "Ready for sale products" },
                new Category { CategoryId = 4, CategoryName = "Packaging", Description = "Packaging materials" }
            );

            // Seed Suppliers
            modelBuilder.Entity<Supplier>().HasData(
                new Supplier { SupplierId = 1, CompanyName = "ABC Manufacturing Ltd", ContactPerson = "John Smith", Email = "john@abcmfg.com", Phone = "+1-555-0101", Address = "123 Industrial Ave", City = "Chicago", Country = "USA" },
                new Supplier { SupplierId = 2, CompanyName = "Global Parts Inc", ContactPerson = "Sarah Johnson", Email = "sarah@globalparts.com", Phone = "+1-555-0102", Address = "456 Commerce St", City = "New York", Country = "USA" },
                new Supplier { SupplierId = 3, CompanyName = "TechSupply Co", ContactPerson = "Mike Chen", Email = "mike@techsupply.com", Phone = "+1-555-0103", Address = "789 Tech Blvd", City = "San Francisco", Country = "USA" }
            );

            // Seed Warehouses
            modelBuilder.Entity<Warehouse>().HasData(
                new Warehouse { WarehouseId = 1, WarehouseName = "Main Warehouse", WarehouseCode = "WH-001", Address = "100 Storage Lane", City = "Chicago", Country = "USA", ManagerName = "Tom Wilson", Phone = "+1-555-0201", Capacity = 10000 },
                new Warehouse { WarehouseId = 2, WarehouseName = "Distribution Center", WarehouseCode = "WH-002", Address = "200 Logistics Way", City = "Dallas", Country = "USA", ManagerName = "Lisa Brown", Phone = "+1-555-0202", Capacity = 15000 }
            );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, ProductName = "Steel Sheet 4x8", SKU = "STL-001", Description = "High-grade steel sheet", UnitPrice = 45.99m, ReorderLevel = 100, CategoryId = 1, SupplierId = 1 },
                new Product { ProductId = 2, ProductName = "Aluminum Rod 1/2 inch", SKU = "ALU-001", Description = "Aluminum rod for manufacturing", UnitPrice = 12.50m, ReorderLevel = 200, CategoryId = 1, SupplierId = 1 },
                new Product { ProductId = 3, ProductName = "Electric Motor 5HP", SKU = "MOT-001", Description = "Industrial electric motor", UnitPrice = 299.99m, ReorderLevel = 25, CategoryId = 2, SupplierId = 2 },
                new Product { ProductId = 4, ProductName = "Control Panel Unit", SKU = "CTL-001", Description = "Electronic control panel", UnitPrice = 549.00m, ReorderLevel = 15, CategoryId = 2, SupplierId = 3 },
                new Product { ProductId = 5, ProductName = "Assembly Machine A1", SKU = "ASM-001", Description = "Automated assembly machine", UnitPrice = 15000.00m, ReorderLevel = 5, CategoryId = 3, SupplierId = 2 },
                new Product { ProductId = 6, ProductName = "Cardboard Box Large", SKU = "PKG-001", Description = "Large shipping box", UnitPrice = 2.50m, ReorderLevel = 500, CategoryId = 4, SupplierId = 1 }
            );

            // Seed Inventory
            modelBuilder.Entity<Inventory>().HasData(
                new Inventory { InventoryId = 1, ProductId = 1, WarehouseId = 1, QuantityOnHand = 500, QuantityReserved = 50, UnitCost = 40.00m },
                new Inventory { InventoryId = 2, ProductId = 2, WarehouseId = 1, QuantityOnHand = 800, QuantityReserved = 100, UnitCost = 10.00m },
                new Inventory { InventoryId = 3, ProductId = 3, WarehouseId = 1, QuantityOnHand = 45, QuantityReserved = 5, UnitCost = 250.00m },
                new Inventory { InventoryId = 4, ProductId = 4, WarehouseId = 2, QuantityOnHand = 30, QuantityReserved = 10, UnitCost = 450.00m },
                new Inventory { InventoryId = 5, ProductId = 5, WarehouseId = 2, QuantityOnHand = 8, QuantityReserved = 2, UnitCost = 12000.00m },
                new Inventory { InventoryId = 6, ProductId = 6, WarehouseId = 1, QuantityOnHand = 2000, QuantityReserved = 200, UnitCost = 1.80m }
            );

            // Seed User (Admin)
            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, Username = "admin", Email = "admin@erp.com", PasswordHash = "AQAAAAEAACcQAAAAEJx...", FirstName = "System", LastName = "Administrator", Role = "Admin" },
                new User { UserId = 2, Username = "manager", Email = "manager@erp.com", PasswordHash = "AQAAAAEAACcQAAAAEJx...", FirstName = "John", LastName = "Manager", Role = "Manager" }
            );
        }
    }
}
