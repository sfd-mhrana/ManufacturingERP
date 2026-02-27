import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { Inventory } from '../../models/dashboard.model';

@Component({
  selector: 'app-inventory',
  standalone: false,
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.scss']
})
export class InventoryComponent implements OnInit {
  inventoryItems: Inventory[] = [];
  displayedColumns: string[] = ['sku', 'productName', 'warehouseName', 'quantityOnHand', 'quantityAvailable', 'unitCost', 'totalValue', 'status'];
  isLoading = true;

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.loadInventory();
  }

  loadInventory(): void {
    this.apiService.getInventory().subscribe({
      next: (data) => {
        this.inventoryItems = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading inventory', error);
        this.isLoading = false;
      }
    });
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(value);
  }

  getStockStatus(item: Inventory): string {
    if (item.isLowStock) return 'Low Stock';
    if (item.quantityAvailable < 50) return 'Warning';
    return 'In Stock';
  }

  getStatusClass(item: Inventory): string {
    if (item.isLowStock) return 'status-low';
    if (item.quantityAvailable < 50) return 'status-warning';
    return 'status-ok';
  }
}
