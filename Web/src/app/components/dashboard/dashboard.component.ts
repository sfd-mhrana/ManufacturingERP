import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { DashboardData } from '../../models/dashboard.model';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  dashboardData: DashboardData | null = null;
  isLoading = true;

  // Chart data
  categoryChartData: any;
  ordersChartData: any;

  chartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        position: 'bottom' as const
      }
    }
  };

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.loadDashboardData();
  }

  loadDashboardData(): void {
    this.apiService.getDashboard().subscribe({
      next: (data) => {
        this.dashboardData = data;
        this.prepareChartData();
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading dashboard data', error);
        this.isLoading = false;
      }
    });
  }

  prepareChartData(): void {
    if (!this.dashboardData) return;

    // Category Stock Chart (Pie)
    this.categoryChartData = {
      labels: this.dashboardData.stockByCategory.map(c => c.categoryName),
      datasets: [{
        data: this.dashboardData.stockByCategory.map(c => c.totalValue),
        backgroundColor: ['#3f51b5', '#e91e63', '#4caf50', '#ff9800'],
        hoverBackgroundColor: ['#303f9f', '#c2185b', '#388e3c', '#f57c00']
      }]
    };

    // Monthly Orders Chart (Bar)
    this.ordersChartData = {
      labels: this.dashboardData.monthlyOrders.map(o => o.month),
      datasets: [{
        label: 'Order Amount ($)',
        data: this.dashboardData.monthlyOrders.map(o => o.totalAmount),
        backgroundColor: '#3f51b5',
        borderColor: '#303f9f',
        borderWidth: 1
      }]
    };
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(value);
  }

  getActivityIcon(type: string): string {
    switch (type) {
      case 'Stock Update': return 'inventory_2';
      case 'Purchase Order': return 'shopping_cart';
      case 'Low Stock Alert': return 'warning';
      case 'Inventory Count': return 'fact_check';
      case 'New Product': return 'add_circle';
      default: return 'info';
    }
  }

  getActivityColor(type: string): string {
    switch (type) {
      case 'Stock Update': return 'primary';
      case 'Purchase Order': return 'accent';
      case 'Low Stock Alert': return 'warn';
      case 'Inventory Count': return 'primary';
      case 'New Product': return 'accent';
      default: return 'primary';
    }
  }
}
