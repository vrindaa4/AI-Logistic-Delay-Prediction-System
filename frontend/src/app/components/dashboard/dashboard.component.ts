import { Component, OnInit } from '@angular/core';
import { CommonModule, SlicePipe, DatePipe } from '@angular/common';
import { LogisticsService } from '../../services/logistics.service';
import { Shipment } from '../../models/logistics.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, SlicePipe, DatePipe],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  shipments: Shipment[] = [];
  totalShipments = 0;
  delayedShipments = 0;
  inTransitShipments = 0;
  deliveredShipments = 0;
  loading = true;

  sortColumn: string = 'shipmentNumber';
  sortDirection: 'asc' | 'desc' = 'asc';

  constructor(private logisticsService: LogisticsService) {}

  ngOnInit(): void {
    this.loadDashboardData();
  }

  loadDashboardData(): void {
    this.loading = true;
    this.logisticsService.getShipments().subscribe({
      next: (data: Shipment[]) => {
        this.shipments = data;
        this.calculateMetrics();
        this.sortShipments();
        this.loading = false;
      },
      error: (error: any) => {
        console.error('Error loading shipments:', error);
        this.loading = false;
      }
    });
  }

  calculateMetrics(): void {
    this.totalShipments = this.shipments.length;
    this.delayedShipments = this.shipments.filter(s => s.status === 'delayed').length;
    this.inTransitShipments = this.shipments.filter(s => s.status === 'in-transit').length;
    this.deliveredShipments = this.shipments.filter(s => s.status === 'delivered').length;
  }

  sortShipments(): void {
    this.shipments.sort((a, b) => {
      let aValue: any;
      let bValue: any;
      switch (this.sortColumn) {
        case 'shipmentNumber': aValue = a.shipmentNumber; bValue = b.shipmentNumber; break;
        case 'origin':         aValue = a.origin;         bValue = b.origin;         break;
        case 'destination':    aValue = a.destination;    bValue = b.destination;    break;
        case 'status':         aValue = a.status;         bValue = b.status;         break;
        case 'estimatedDelivery':
          aValue = new Date(a.estimatedDeliveryDateUtc).getTime();
          bValue = new Date(b.estimatedDeliveryDateUtc).getTime();
          break;
        default: return 0;
      }
      if (typeof aValue === 'string') {
        aValue = aValue.toLowerCase(); bValue = bValue.toLowerCase();
        return this.sortDirection === 'asc' ? aValue.localeCompare(bValue) : bValue.localeCompare(aValue);
      }
      return this.sortDirection === 'asc' ? aValue - bValue : bValue - aValue;
    });
  }

  onSortColumnClick(column: string): void {
    if (this.sortColumn === column) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortColumn = column;
      this.sortDirection = 'asc';
    }
    this.sortShipments();
  }

  getSortIndicator(column: string): string {
    if (this.sortColumn !== column) return '';
    return this.sortDirection === 'asc' ? ' ▲' : ' ▼';
  }

  getStatusClass(status: string): string {
    return `badge-${status}`;
  }
}
