import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, RouterLink, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from './services/auth.service';
import { NotificationService } from './services/notification.service';
import { NotificationBellComponent } from './components/notification-bell/notification-bell.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink, FormsModule,NotificationBellComponent],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Logistics Delay Prediction System';

constructor(private authService: AuthService,
   private notificationService: NotificationService,
   private router: Router
  ) {}
  ngOnInit(): void {
    if (this.authService.isLoggedIn()) {
      this.notificationService.connect();
    }
  }

  isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }
  getUserName(): string {
    return this.authService.getName() || 'User';
  }

  get isAdmin(): boolean { return this.authService.isAdmin(); }

  onLogout(): void {
    this.notificationService.disconnect();
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}