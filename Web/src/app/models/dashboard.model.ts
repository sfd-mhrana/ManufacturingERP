export interface DashboardData {
  totalProducts: number;
  totalSuppliers: number;
  totalWarehouses: number;
  lowStockItems: number;
  pendingOrders: number;
  totalInventoryValue: number;
  stockByCategory: CategoryStock[];
  recentActivities: RecentActivity[];
  topProducts: TopProduct[];
  monthlyOrders: MonthlyOrder[];
}

export interface CategoryStock {
  categoryName: string;
  totalQuantity: number;
  totalValue: number;
}

export interface RecentActivity {
  activityType: string;
  description: string;
  timestamp: Date;
}

export interface TopProduct {
  productName: string;
  quantityInStock: number;
  totalValue: number;
}

export interface MonthlyOrder {
  month: string;
  orderCount: number;
  totalAmount: number;
}

export interface Product {
  productId: number;
  productName: string;
  sku: string;
  description: string;
  unitPrice: number;
  reorderLevel: number;
  categoryId: number;
  categoryName: string;
  supplierId: number;
  supplierName: string;
  isActive: boolean;
  createdAt: Date;
}

export interface Inventory {
  inventoryId: number;
  productId: number;
  productName: string;
  sku: string;
  warehouseId: number;
  warehouseName: string;
  quantityOnHand: number;
  quantityReserved: number;
  quantityAvailable: number;
  unitCost: number;
  totalValue: number;
  lastStockUpdate: Date;
  isLowStock: boolean;
}

export interface Supplier {
  supplierId: number;
  companyName: string;
  contactPerson: string;
  email: string;
  phone: string;
  address: string;
  city: string;
  country: string;
  isActive: boolean;
}

export interface Category {
  categoryId: number;
  categoryName: string;
  description: string;
  isActive: boolean;
}

export interface Warehouse {
  warehouseId: number;
  warehouseName: string;
  warehouseCode: string;
  address: string;
  city: string;
  country: string;
  managerName: string;
  phone: string;
  capacity: number;
  isActive: boolean;
}
