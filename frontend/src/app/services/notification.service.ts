import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { environment } from '../../environments/environment';
import { AppNotification } from '../models/notification.model';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private apiUrl = `${environment.apiUrl}/notification`;
  private hubConnection: HubConnection | null = null;

  private notificationsSubject = new BehaviorSubject<AppNotification[]>([]);
  notifications$: Observable<AppNotification[]> = this.notificationsSubject.asObservable();

  private unreadCountSubject = new BehaviorSubject<number>(0);
  unreadCount$: Observable<number> = this.unreadCountSubject.asObservable();

  private alertSubject = new Subject<AppNotification>();
  alertReceived$: Observable<AppNotification> = this.alertSubject.asObservable();

  constructor(private http: HttpClient, private authService: AuthService) {}

  connect(): void {
    if (this.hubConnection) return;

    const token = this.authService.getToken();
    if (!token) return;

    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.hubUrl}/notifications`, {
        accessTokenFactory: () => token,
        withCredentials: false
      })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Warning)
      .build();

    this.hubConnection.on('ReceiveNotification', (notification: AppNotification) => {
      this.notificationsSubject.next([notification, ...this.notificationsSubject.value]);
      this.unreadCountSubject.next(this.unreadCountSubject.value + 1);
      this.alertSubject.next(notification);
    });

    this.hubConnection
      .start()
      .then(() => {
        this.getNotifications().subscribe(list => this.notificationsSubject.next(list));
        this.refreshUnreadCount();
      })
      .catch(err => console.error('Notification hub connection failed:', err));
  }

  disconnect(): void {
    this.hubConnection?.stop();
    this.hubConnection = null;
    this.notificationsSubject.next([]);
    this.unreadCountSubject.next(0);
  }

  getNotifications(): Observable<AppNotification[]> {
    return this.http.get<AppNotification[]>(this.apiUrl);
  }

  refreshUnreadCount(): void {
    this.http
      .get<{ count: number }>(`${this.apiUrl}/unread-count`)
      .subscribe(res => this.unreadCountSubject.next(res.count));
  }

  markAsRead(id: number): void {
    this.http.put(`${this.apiUrl}/${id}/read`, {}).subscribe(() => {
      const updated = this.notificationsSubject.value.map(n =>
        n.id === id ? { ...n, isRead: true } : n
      );
      this.notificationsSubject.next(updated);
      this.unreadCountSubject.next(Math.max(0, this.unreadCountSubject.value - 1));
    });
  }

  markAllAsRead(): void {
    this.http.put(`${this.apiUrl}/mark-all-read`, {}).subscribe(() => {
      const updated = this.notificationsSubject.value.map(n => ({ ...n, isRead: true }));
      this.notificationsSubject.next(updated);
      this.unreadCountSubject.next(0);
    });
  }
}