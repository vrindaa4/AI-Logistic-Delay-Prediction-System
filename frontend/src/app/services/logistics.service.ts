import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Shipment, PredictionRequest, PredictionResponse } from '../models/logistics.model';

@Injectable({
  providedIn: 'root'
})
export class LogisticsService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

 
  getShipments(): Observable<Shipment[]> {
    console.log('Fetching shipments from:', `${this.apiUrl}/shipment`);
    return this.http.get<Shipment[]>(`${this.apiUrl}/shipment`);
  }

  
  getShipment(id: number): Observable<Shipment> {
    return this.http.get<Shipment>(`${this.apiUrl}/shipment/${id}`);
  }


  createShipment(shipment: Partial<Shipment>): Observable<Shipment> {
    return this.http.post<Shipment>(`${this.apiUrl}/shipment`, shipment);
  }

  
  updateShipment(id: number, shipment: Partial<Shipment>): Observable<Shipment> {
    return this.http.put<Shipment>(`${this.apiUrl}/shipment/${id}`, shipment);
  }


  deleteShipment(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/shipment/${id}`);
  }


  predictDelay(request: PredictionRequest): Observable<PredictionResponse> {
    return this.http.post<PredictionResponse>(`${this.apiUrl}/shipment/predict`, request);
  }
}
