export interface Shipment {
  id: number;
  shipmentNumber: string;
  origin: string;
  destination: string;
  status: string;
  carrier: string;
  trackingNumber: string;
  weight: number;
  estimatedDeliveryDateUtc: Date;
  createdAtUtc: Date;
  deliveredAtUtc?: Date;
}

export interface PredictionRequest {
  origin: string;
  destination: string;
  weight: number;
  carrier: string;
}

export interface PredictionResponse {
  probability: number;
  estimatedDelayDays: number;
  riskLevel: 'low' | 'medium' | 'high';
  recommendation: string;
}
