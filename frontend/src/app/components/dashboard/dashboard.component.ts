import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LogisticsService } from '../../services/logistics.service';
import { Shipment } from '../../models/logistics.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
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

  getStatusClass(status: string): string {
    return `badge-${status}`;    
  }
}
