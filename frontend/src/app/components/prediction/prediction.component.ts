import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LogisticsService } from '../../services/logistics.service';
import { NotificationService } from '../../services/notification.service';
import { PredictionRequest, PredictionResponse } from '../../models/logistics.model';

@Component({
  selector: 'app-prediction',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './prediction.component.html',
  styleUrls: ['./prediction.component.scss']
})
export class PredictionComponent {
  predictionForm: PredictionRequest = {
    origin: '',
    destination: '',
    weight: 0,
    carrier: ''
  };

  result: PredictionResponse | null = null;
  loading = false;
  creating = false;
  errorMessage = '';
  successMessage = '';
  delayNotificationMessage = '';

  constructor(
    private logisticsService: LogisticsService,
    private notificationService: NotificationService
  ) {}

  onSubmit(): void {
    if (!this.validateForm()) return;
    this.loading = true;
    this.errorMessage = '';
    this.delayNotificationMessage = '';

    this.logisticsService.predictDelay(this.predictionForm).subscribe({
      next: (response: PredictionResponse) => {
        this.result = response;
        this.loading = false;

        // If delay predicted, show notification banner
        const isDelayed = response.riskLevel?.toLowerCase() !== 'low';
        if (isDelayed) {
          this.delayNotificationMessage = 'Shipment is predicted to be delayed.';
          // Refresh notification bell count (backend already saved it)
          this.notificationService.refreshUnreadCount();
        }
      },
      error: (error: any) => {
        console.error('Error predicting delay:', error);
        this.errorMessage = 'Failed to predict delay. Please try again.';
        this.loading = false;
      }
    });
  }

  createShipment(): void {
    if (!this.validateForm()) return;
    this.creating = true;
    this.errorMessage = '';
    this.successMessage = '';

    const dto = {
      origin: this.predictionForm.origin,
      destination: this.predictionForm.destination,
      carrier: this.predictionForm.carrier,
      weight: this.predictionForm.weight
    };

    this.logisticsService.createShipment(dto).subscribe({
      next: () => {
        this.successMessage = 'Shipment created successfully! Check your dashboard for details.';
        this.creating = false;
        // Refresh notification bell (backend sends creation notification via SignalR)
        this.notificationService.refreshUnreadCount();
      },
      error: (error: any) => {
        console.error('Error creating shipment:', error);
        this.errorMessage = 'Failed to create shipment. Please try again.';
        this.creating = false;
      }
    });
  }

  validateForm(): boolean {
    if (!this.predictionForm.origin.trim()) {
      this.errorMessage = 'Please enter origin';
      return false;
    }
    if (!this.predictionForm.destination.trim()) {
      this.errorMessage = 'Please enter destination';
      return false;
    }
    if (this.predictionForm.weight <= 0) {
      this.errorMessage = 'Weight must be greater than 0';
      return false;
    }
    if (!this.predictionForm.carrier.trim()) {
      this.errorMessage = 'Please enter carrier';
      return false;
    }
    return true;
  }

  reset(): void {
    this.predictionForm = { origin: '', destination: '', weight: 0, carrier: '' };
    this.result = null;
    this.errorMessage = '';
    this.successMessage = '';
    this.delayNotificationMessage = '';
  }

  getRiskLevelClass(riskLevel: string): string {
    return `risk-${riskLevel}`;
  }
}