import { Component, ElementRef, HostListener, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { NotificationService } from '../../services/notification.service';
import { AppNotification } from '../../models/notification.model';

@Component({
  selector: 'app-notification-bell',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './notification-bell.component.html',
  styleUrls: ['./notification-bell.component.scss']
})
export class NotificationBellComponent implements OnInit, OnDestroy {
  notifications: AppNotification[] = [];
  unreadCount = 0;
  dropdownOpen = false;
  toastNotification: AppNotification | null = null;

  private subscriptions = new Subscription();
  private toastTimer?: ReturnType<typeof setTimeout>;

  constructor(
    private notificationService: NotificationService,
    private router: Router,
    private elementRef: ElementRef
  ) {}

  ngOnInit(): void {
    this.subscriptions.add(
      this.notificationService.notifications$.subscribe(list => (this.notifications = list))
    );
    this.subscriptions.add(
      this.notificationService.unreadCount$.subscribe(count => (this.unreadCount = count))
    );
    this.subscriptions.add(
      this.notificationService.alertReceived$.subscribe(n => this.showToast(n))
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
    if (this.toastTimer) clearTimeout(this.toastTimer);
  }

  toggleDropdown(): void {
    this.dropdownOpen = !this.dropdownOpen;
  }

  onItemClick(notification: AppNotification): void {
    if (!notification.isRead) {
      this.notificationService.markAsRead(notification.id);
    }
    this.dropdownOpen = false;
    if (notification.shipmentId) {
      this.router.navigate(['/shipments']);
    }
  }

  markAllAsRead(): void {
    this.notificationService.markAllAsRead();
  }

  dismissToast(): void {
    this.toastNotification = null;
    if (this.toastTimer) clearTimeout(this.toastTimer);
  }

  private showToast(notification: AppNotification): void {
    this.toastNotification = notification;
    if (this.toastTimer) clearTimeout(this.toastTimer);
    this.toastTimer = setTimeout(() => (this.toastNotification = null), 6000);
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    if (this.dropdownOpen && !this.elementRef.nativeElement.contains(event.target)) {
      this.dropdownOpen = false;
    }
  }
}