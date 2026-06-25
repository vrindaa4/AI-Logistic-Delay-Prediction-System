export interface AppNotification {
  id: number;
  shipmentId?: number | null;
  shipmentNumber?: string | null;
  title: string;
  message: string;
  type: string;
  isRead: boolean;
  createdAtUtc: string;
}