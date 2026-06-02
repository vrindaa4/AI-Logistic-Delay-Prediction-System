import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LogisticsService } from '../../services/logistics.service';
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
  submitted = false;
  errorMessage = '';

  constructor(private logisticsService: LogisticsService) {}

  onSubmit(): void {
    if (this.validateForm()) {
      this.loading = true;
      this.errorMessage = '';
      
      this.logisticsService.predictDelay(this.predictionForm).subscribe({
        next: (response: PredictionResponse) => {
          this.result = response;
          this.loading = false;
        },
        error: (error: any) => {
          console.error('Error predicting delay:', error);
          this.errorMessage = 'Failed to predict delay. Please try again.';
          this.loading = false;
        }
      });
    }
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
    this.predictionForm = {
      origin: '',
      destination: '',
      weight: 0,
      carrier: ''
    };
    this.result = null;
    this.submitted = false;
    this.errorMessage = '';
  }

  getRiskLevelClass(riskLevel: string): string {
    return `risk-${riskLevel}`;
  }
}
