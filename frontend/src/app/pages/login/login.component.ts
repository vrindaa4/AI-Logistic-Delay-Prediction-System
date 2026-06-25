import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

  email: string = '';
  password: string = '';

  selectedRole: 'User' | 'Admin' = 'User';
  errorMessage: string = '';
  loading: boolean = false;

  constructor(
    private authService: AuthService,
    private router: Router,
     private notificationService: NotificationService,
  ) {}

  selectRole(role: 'User' | 'Admin'): void {
    this.selectedRole = role;
    this.errorMessage = '';
    
  }

  login(): void {
  this.errorMessage = '';
  this.loading = true;

  this.authService.login({ email: this.email, password: this.password })
    .subscribe({
      next: (response: any) => {
        this.loading = false;

        if (response.role !== this.selectedRole) {
          this.errorMessage = this.selectedRole === 'Admin'
            ? 'This account does not have Admin access.'
            : 'This is an Admin account. Please use the Admin tab to log in.';
          return;
        }

        this.authService.saveToken(response.token);
        this.authService.saveRole(response.role);
        this.authService.saveName(response.name);        
        this.notificationService.connect();

        if (response.role === 'Admin') {
          this.router.navigate(['/dashboard']);
        } else {
          this.router.navigate(['/shipments']);
        }
      },
      error: (err) => {
        this.loading = false;
        console.error(err);
        this.errorMessage = 'Invalid email or password.';
      }
    });
}
}