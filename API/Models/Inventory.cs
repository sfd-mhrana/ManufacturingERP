using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManufacturingERP.API.Models
{
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }

        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }

        public int WarehouseId { get; set; }
        public virtual Warehouse? Warehouse { get; set; }

        public int QuantityOnHand { get; set; }

        public int QuantityReserved { get; set; }

        public int QuantityAvailable => QuantityOnHand - QuantityReserved;

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalValue => QuantityOnHand * UnitCost;

        public DateTime LastStockUpdate { get; set; } = DateTime.UtcNow;

        public DateTime? LastCountDate { get; set; }
    }
}
