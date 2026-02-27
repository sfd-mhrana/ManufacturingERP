import { Component } from '@angular/core';

@Component({
  selector: 'app-sidenav',
  standalone: false,
  template: `
    <mat-sidenav-container class="sidenav-container">
      <mat-sidenav mode="side" opened class="sidenav">
        <div class="sidenav-header">
          <mat-icon class="logo-icon">factory</mat-icon>
          <span class="logo-text">Manufacturing ERP</span>
        </div>

        <mat-nav-list>
          <a mat-list-item routerLink="/dashboard" routerLinkActive="active">
            <mat-icon matListItemIcon>dashboard</mat-icon>
            <span matListItemTitle>Dashboard</span>
          </a>
          <a mat-list-item routerLink="/inventory" routerLinkActive="active">
            <mat-icon matListItemIcon>inventory_2</mat-icon>
            <span matListItemTitle>Inventory</span>
          </a>
          <a mat-list-item routerLink="/products" routerLinkActive="active">
            <mat-icon matListItemIcon>category</mat-icon>
            <span matListItemTitle>Products</span>
          </a>
          <a mat-list-item disabled>
            <mat-icon matListItemIcon>local_shipping</mat-icon>
            <span matListItemTitle>Suppliers</span>
          </a>
          <a mat-list-item disabled>
            <mat-icon matListItemIcon>warehouse</mat-icon>
            <span matListItemTitle>Warehouses</span>
          </a>
          <a mat-list-item disabled>
            <mat-icon matListItemIcon>receipt_long</mat-icon>
            <span matListItemTitle>Purchase Orders</span>
          </a>
          <a mat-list-item disabled>
            <mat-icon matListItemIcon>bar_chart</mat-icon>
            <span matListItemTitle>Reports</span>
          </a>
        </mat-nav-list>

        <div class="sidenav-footer">
          <mat-divider></mat-divider>
          <a mat-list-item disabled>
            <mat-icon matListItemIcon>settings</mat-icon>
            <span matListItemTitle>Settings</span>
          </a>
          <a mat-list-item disabled>
            <mat-icon matListItemIcon>logout</mat-icon>
            <span matListItemTitle>Logout</span>
          </a>
        </div>
      </mat-sidenav>

      <mat-sidenav-content class="main-content">
        <mat-toolbar color="primary" class="toolbar">
          <button mat-icon-button>
            <mat-icon>menu</mat-icon>
          </button>
          <span class="toolbar-spacer"></span>
          <button mat-icon-button>
            <mat-icon matBadge="3" matBadgeColor="warn">notifications</mat-icon>
          </button>
          <button mat-icon-button [matMenuTriggerFor]="userMenu">
            <mat-icon>account_circle</mat-icon>
          </button>
          <mat-menu #userMenu="matMenu">
            <button mat-menu-item>
              <mat-icon>person</mat-icon>
              <span>Profile</span>
            </button>
            <button mat-menu-item>
              <mat-icon>settings</mat-icon>
              <span>Settings</span>
            </button>
            <mat-divider></mat-divider>
            <button mat-menu-item>
              <mat-icon>logout</mat-icon>
              <span>Logout</span>
            </button>
          </mat-menu>
        </mat-toolbar>

        <div class="content-area">
          <router-outlet></router-outlet>
        </div>
      </mat-sidenav-content>
    </mat-sidenav-container>
  `,
  styles: [`
    .sidenav-container {
      height: 100vh;
    }

    .sidenav {
      width: 260px;
      background: #fff;
      border-right: 1px solid #e0e0e0;
    }

    .sidenav-header {
      display: flex;
      align-items: center;
      padding: 20px 16px;
      border-bottom: 1px solid #e0e0e0;
    }

    .logo-icon {
      font-size: 32px;
      width: 32px;
      height: 32px;
      color: #3f51b5;
      margin-right: 12px;
    }

    .logo-text {
      font-size: 18px;
      font-weight: 500;
      color: #333;
    }

    mat-nav-list a {
      margin: 4px 8px;
      border-radius: 8px;
    }

    mat-nav-list a.active {
      background: #e8eaf6;
      color: #3f51b5;
    }

    .sidenav-footer {
      position: absolute;
      bottom: 0;
      width: 100%;
      background: #fff;
    }

    .main-content {
      background: #f5f5f5;
    }

    .toolbar {
      position: sticky;
      top: 0;
      z-index: 100;
    }

    .toolbar-spacer {
      flex: 1;
    }

    .content-area {
      min-height: calc(100vh - 64px);
    }
  `]
})
export class SidenavComponent {}
