import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LogisticsService } from '../../services/logistics.service';
import { Shipment } from '../../models/logistics.model';

@Component({
  selector: 'app-shipment-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './shipment-list.component.html',
  styleUrls: ['./shipment-list.component.scss']
})
export class ShipmentListComponent implements OnInit {
  shipments: Shipment[] = [];
  filteredShipments: Shipment[] = [];
  loading = true;
  searchTerm = '';
  selectedStatus = '';

  constructor(private logisticsService: LogisticsService) {}

  ngOnInit(): void {
    this.loadShipments();
  }

  loadShipments(): void {
    this.loading = true;
    this.logisticsService.getShipments().subscribe({
      next: (data: Shipment[]) => {
        this.shipments = data;
        this.applyFilters();
        this.loading = false;
      },
      error: (error: any) => {
        console.error('Error loading shipments:', error);
        this.loading = false;
      }
    });
  }

  applyFilters(): void {
    this.filteredShipments = this.shipments.filter(shipment => {
      const matchesSearch = 
        shipment.shipmentNumber.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        shipment.origin.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        shipment.destination.toLowerCase().includes(this.searchTerm.toLowerCase());

      const matchesStatus = !this.selectedStatus || shipment.status === this.selectedStatus;

      return matchesSearch && matchesStatus;
    });
  }

  onSearchChange(): void {
    this.applyFilters();
  }

  onStatusChange(): void {
    this.applyFilters();
  }

  deleteShipment(id: number): void {
    if (confirm('Are you sure you want to delete this shipment?')) {
      this.logisticsService.deleteShipment(id).subscribe({
        next: () => {
          this.loadShipments();
        },
        error: (error: any) => {
          console.error('Error deleting shipment:', error);
        }
      });
    }
  }

  getStatusClass(status: string): string {
    return `badge-${status}`;
  }
}
