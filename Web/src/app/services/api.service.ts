import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { DashboardData, Product, Inventory, Supplier, Category, Warehouse } from '../models/dashboard.model';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = 'https://localhost:7001/api';

  constructor(private http: HttpClient) {}

  // Dashboard
  getDashboard(): Observable<DashboardData> {
    // Return mock data for demo purposes (since we're not running the API)
    return of(this.getMockDashboardData());
  }

  // Products
  getProducts(): Observable<Product[]> {
    return of(this.getMockProducts());
  }

  getProduct(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.baseUrl}/products/${id}`);
  }

  // Inventory
  getInventory(): Observable<Inventory[]> {
    return of(this.getMockInventory());
  }

  getLowStockItems(): Observable<Inventory[]> {
    return this.http.get<Inventory[]>(`${this.baseUrl}/inventory/low-stock`);
  }

  // Suppliers
  getSuppliers(): Observable<Supplier[]> {
    return of(this.getMockSuppliers());
  }

  // Categories
  getCategories(): Observable<Category[]> {
    return of(this.getMockCategories());
  }

  // Warehouses
  getWarehouses(): Observable<Warehouse[]> {
    return of(this.getMockWarehouses());
  }

  // Mock Data for Demo
  private getMockDashboardData(): DashboardData {
    return {
      totalProducts: 156,
      totalSuppliers: 24,
      totalWarehouses: 3,
      lowStockItems: 8,
      pendingOrders: 12,
      totalInventoryValue: 1547890.50,
      stockByCategory: [
        { categoryName: 'Raw Materials', totalQuantity: 1300, totalValue: 52000 },
        { categoryName: 'Components', totalQuantity: 75, totalValue: 52500 },
        { categoryName: 'Finished Goods', totalQuantity: 8, totalValue: 120000 },
        { categoryName: 'Packaging', totalQuantity: 2000, totalValue: 5000 }
      ],
      topProducts: [
        { productName: 'Assembly Machine A1', quantityInStock: 8, totalValue: 96000 },
        { productName: 'Control Panel Unit', quantityInStock: 30, totalValue: 13500 },
        { productName: 'Electric Motor 5HP', quantityInStock: 45, totalValue: 11250 },
        { productName: 'Steel Sheet 4x8', quantityInStock: 500, totalValue: 20000 },
        { productName: 'Aluminum Rod 1/2 inch', quantityInStock: 800, totalValue: 8000 }
      ],
      recentActivities: [
        { activityType: 'Stock Update', description: 'Steel Sheet 4x8 - Added 100 units', timestamp: new Date() },
        { activityType: 'Purchase Order', description: 'PO-2025-001 - Submitted to ABC Manufacturing Ltd', timestamp: new Date(Date.now() - 3600000) },
        { activityType: 'Low Stock Alert', description: 'Electric Motor 5HP - Below reorder level', timestamp: new Date(Date.now() - 7200000) },
        { activityType: 'Inventory Count', description: 'Main Warehouse - Cycle count completed', timestamp: new Date(Date.now() - 86400000) },
        { activityType: 'New Product', description: 'Control Panel Unit - Added to catalog', timestamp: new Date(Date.now() - 172800000) }
      ],
      monthlyOrders: [
        { month: 'Jan', orderCount: 45, totalAmount: 125000 },
        { month: 'Feb', orderCount: 52, totalAmount: 148000 },
        { month: 'Mar', orderCount: 48, totalAmount: 132000 },
        { month: 'Apr', orderCount: 61, totalAmount: 175000 },
        { month: 'May', orderCount: 55, totalAmount: 156000 },
        { month: 'Jun', orderCount: 67, totalAmount: 198000 }
      ]
    };
  }

  private getMockProducts(): Product[] {
    return [
      { productId: 1, productName: 'Steel Sheet 4x8', sku: 'STL-001', description: 'High-grade steel sheet', unitPrice: 45.99, reorderLevel: 100, categoryId: 1, categoryName: 'Raw Materials', supplierId: 1, supplierName: 'ABC Manufacturing Ltd', isActive: true, createdAt: new Date() },
      { productId: 2, productName: 'Aluminum Rod 1/2 inch', sku: 'ALU-001', description: 'Aluminum rod for manufacturing', unitPrice: 12.50, reorderLevel: 200, categoryId: 1, categoryName: 'Raw Materials', supplierId: 1, supplierName: 'ABC Manufacturing Ltd', isActive: true, createdAt: new Date() },
      { productId: 3, productName: 'Electric Motor 5HP', sku: 'MOT-001', description: 'Industrial electric motor', unitPrice: 299.99, reorderLevel: 25, categoryId: 2, categoryName: 'Components', supplierId: 2, supplierName: 'Global Parts Inc', isActive: true, createdAt: new Date() },
      { productId: 4, productName: 'Control Panel Unit', sku: 'CTL-001', description: 'Electronic control panel', unitPrice: 549.00, reorderLevel: 15, categoryId: 2, categoryName: 'Components', supplierId: 3, supplierName: 'TechSupply Co', isActive: true, createdAt: new Date() },
      { productId: 5, productName: 'Assembly Machine A1', sku: 'ASM-001', description: 'Automated assembly machine', unitPrice: 15000.00, reorderLevel: 5, categoryId: 3, categoryName: 'Finished Goods', supplierId: 2, supplierName: 'Global Parts Inc', isActive: true, createdAt: new Date() },
      { productId: 6, productName: 'Cardboard Box Large', sku: 'PKG-001', description: 'Large shipping box', unitPrice: 2.50, reorderLevel: 500, categoryId: 4, categoryName: 'Packaging', supplierId: 1, supplierName: 'ABC Manufacturing Ltd', isActive: true, createdAt: new Date() }
    ];
  }

  private getMockInventory(): Inventory[] {
    return [
      { inventoryId: 1, productId: 1, productName: 'Steel Sheet 4x8', sku: 'STL-001', warehouseId: 1, warehouseName: 'Main Warehouse', quantityOnHand: 500, quantityReserved: 50, quantityAvailable: 450, unitCost: 40.00, totalValue: 20000, lastStockUpdate: new Date(), isLowStock: false },
      { inventoryId: 2, productId: 2, productName: 'Aluminum Rod 1/2 inch', sku: 'ALU-001', warehouseId: 1, warehouseName: 'Main Warehouse', quantityOnHand: 800, quantityReserved: 100, quantityAvailable: 700, unitCost: 10.00, totalValue: 8000, lastStockUpdate: new Date(), isLowStock: false },
      { inventoryId: 3, productId: 3, productName: 'Electric Motor 5HP', sku: 'MOT-001', warehouseId: 1, warehouseName: 'Main Warehouse', quantityOnHand: 45, quantityReserved: 5, quantityAvailable: 40, unitCost: 250.00, totalValue: 11250, lastStockUpdate: new Date(), isLowStock: false },
      { inventoryId: 4, productId: 4, productName: 'Control Panel Unit', sku: 'CTL-001', warehouseId: 2, warehouseName: 'Distribution Center', quantityOnHand: 30, quantityReserved: 10, quantityAvailable: 20, unitCost: 450.00, totalValue: 13500, lastStockUpdate: new Date(), isLowStock: false },
      { inventoryId: 5, productId: 5, productName: 'Assembly Machine A1', sku: 'ASM-001', warehouseId: 2, warehouseName: 'Distribution Center', quantityOnHand: 8, quantityReserved: 2, quantityAvailable: 6, unitCost: 12000.00, totalValue: 96000, lastStockUpdate: new Date(), isLowStock: false },
      { inventoryId: 6, productId: 6, productName: 'Cardboard Box Large', sku: 'PKG-001', warehouseId: 1, warehouseName: 'Main Warehouse', quantityOnHand: 2000, quantityReserved: 200, quantityAvailable: 1800, unitCost: 1.80, totalValue: 3600, lastStockUpdate: new Date(), isLowStock: false }
    ];
  }

  private getMockSuppliers(): Supplier[] {
    return [
      { supplierId: 1, companyName: 'ABC Manufacturing Ltd', contactPerson: 'John Smith', email: 'john@abcmfg.com', phone: '+1-555-0101', address: '123 Industrial Ave', city: 'Chicago', country: 'USA', isActive: true },
      { supplierId: 2, companyName: 'Global Parts Inc', contactPerson: 'Sarah Johnson', email: 'sarah@globalparts.com', phone: '+1-555-0102', address: '456 Commerce St', city: 'New York', country: 'USA', isActive: true },
      { supplierId: 3, companyName: 'TechSupply Co', contactPerson: 'Mike Chen', email: 'mike@techsupply.com', phone: '+1-555-0103', address: '789 Tech Blvd', city: 'San Francisco', country: 'USA', isActive: true }
    ];
  }

  private getMockCategories(): Category[] {
    return [
      { categoryId: 1, categoryName: 'Raw Materials', description: 'Basic materials for manufacturing', isActive: true },
      { categoryId: 2, categoryName: 'Components', description: 'Assembled parts and components', isActive: true },
      { categoryId: 3, categoryName: 'Finished Goods', description: 'Ready for sale products', isActive: true },
      { categoryId: 4, categoryName: 'Packaging', description: 'Packaging materials', isActive: true }
    ];
  }

  private getMockWarehouses(): Warehouse[] {
    return [
      { warehouseId: 1, warehouseName: 'Main Warehouse', warehouseCode: 'WH-001', address: '100 Storage Lane', city: 'Chicago', country: 'USA', managerName: 'Tom Wilson', phone: '+1-555-0201', capacity: 10000, isActive: true },
      { warehouseId: 2, warehouseName: 'Distribution Center', warehouseCode: 'WH-002', address: '200 Logistics Way', city: 'Dallas', country: 'USA', managerName: 'Lisa Brown', phone: '+1-555-0202', capacity: 15000, isActive: true }
    ];
  }
}
