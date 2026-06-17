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
  delayProbability: number;
  estimatedDelayDays: number;
  riskLevel: 'LOW' | 'MEDIUM' | 'HIGH' | 'low' | 'medium' | 'high';
  recommendation: string;
  aiExplanation: string;
  predictionSource: string;
  trafficData?: {
    durationInTraffic: number;
    distance: number;
    trafficCondition: string;
    estimatedDelayMinutes: number;
    weatherCondition?: string;
    weatherRiskScore?: number;
    dataSource: string;
  };
  factors?: Record<string, string>;
}