import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LogisticsService } from '../../services/logistics.service';
import { AuthService } from '../../services/auth.service';
import { Shipment } from '../../models/logistics.model';
import { Router} from '@angular/router';

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
  statusMap: { [id: number]: string } = {};
  updateMessage = '';
  readonly statuses = ['pending', 'in-transit', 'delayed', 'delivered'];

  constructor(
    private logisticsService: LogisticsService,
    private router: Router,
    private authService: AuthService
  ) {}

  get isAdmin(): boolean {
    return this.authService.isAdmin();
  }

  ngOnInit(): void {
    this.loadShipments();
  }

  loadShipments(): void {
    this.loading = true;
    this.logisticsService.getShipments().subscribe({
      next: (data: Shipment[]) => {
        this.shipments = data;
        data.forEach(s => (this.statusMap[s.id] = s.status));
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
      const term = this.searchTerm.toLowerCase();
      const matchesSearch =
        shipment.shipmentNumber.toLowerCase().includes(term) ||
        (shipment.trackingNumber ?? '').toLowerCase().includes(term) ||
        shipment.origin.toLowerCase().includes(term) ||
        shipment.destination.toLowerCase().includes(term);

      const matchesStatus = !this.selectedStatus || shipment.status === this.selectedStatus;

      return matchesSearch && matchesStatus;
    });
  }
  viewDetail(id: number): void {
  this.router.navigate(['/shipments', id]);
}
  onSearchChange(): void { this.applyFilters(); }
  onStatusChange(): void { this.applyFilters(); }

  deleteShipment(id: number): void {
    if (confirm('Are you sure you want to delete this shipment?')) {
      this.logisticsService.deleteShipment(id).subscribe({
        next: () => { this.loadShipments(); },
        error: (error: any) => { console.error('Error deleting shipment:', error); }
      });
    }
  }
  updateStatus(shipment: Shipment): void {
  const newStatus = this.statusMap[shipment.id];
  this.logisticsService.updateShipmentStatus(shipment.id, newStatus).subscribe({
    next: () => {
      shipment.status = newStatus;
      this.updateMessage = `Status updated for ${shipment.shipmentNumber}`;
      setTimeout(() => (this.updateMessage = ''), 3000);
    },
    error: () => {
      this.updateMessage = 'Failed to update status.';
    }
  });
}

  getStatusClass(status: string): string {
    return `badge-${status}`;
  }
}
