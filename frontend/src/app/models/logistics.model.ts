export interface Shipment {
  id: number;
  shipmentNumber: string;
  trackingNumber: string;
  origin: string;
  destination: string;
  carrier: string;
  status: string;
  weight: number;
  estimatedDeliveryDateUtc: string;
  createdAtUtc: string;
  deliveredAtUtc?: string;
  userId?: number;
}
export interface TrafficData {
  distance: number;
  trafficCondition: string;
  estimatedDelayMinutes: number;
  weatherCondition?: string;
  dataSource?: string;
}
export interface PredictionRequest {
  origin: string;
  destination: string;
  carrier: string;
  weight: number;
}

export interface PredictionResponse {
  delayProbability: number;
  estimatedDelayDays: number;
  riskLevel: string;
  recommendation: string;
  aiExplanation: string;
  predictionSource: string;
  trafficData?: TrafficData;
  factors: { [key: string]: string };
}
