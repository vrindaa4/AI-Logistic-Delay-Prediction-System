import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Shipment, PredictionRequest, PredictionResponse } from '../models/logistics.model';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class LogisticsService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient,
    private authService: AuthService) {}

 
 getShipments(): Observable<Shipment[]> {
    const url = this.authService.isAdmin()
      ? `${this.apiUrl}/shipment`
      : `${this.apiUrl}/shipment/my`;

    return this.http.get<Shipment[]>(url);
  }

  getShipment(id: number): Observable<Shipment> {
    const url = this.authService.isAdmin()
      ? `${this.apiUrl}/shipment/${id}`
      : `${this.apiUrl}/shipment/my/${id}`;

    return this.http.get<Shipment>(url);
  }


  createShipment(shipment: Partial<Shipment>): Observable<Shipment> {
    return this.http.post<Shipment>(`${this.apiUrl}/shipment`, shipment);
  }

  
  updateShipment(id: number, shipment: Partial<Shipment>): Observable<Shipment> {
    return this.http.put<Shipment>(`${this.apiUrl}/shipment/${id}`, shipment);
  }
  updateShipmentStatus(id: number, status: string): Observable<void> {
  return this.http.put<void>(`${this.apiUrl}/shipment/${id}/status`, { status });
}


  deleteShipment(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/shipment/${id}`);
  }


  predictDelay(request: PredictionRequest): Observable<PredictionResponse> {
    return this.http.post<PredictionResponse>(`${this.apiUrl}/shipment/predict`, request);
  }
}
