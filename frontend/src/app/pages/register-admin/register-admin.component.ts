import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register-admin',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register-admin.component.html',
  styleUrls: ['./register-admin.component.scss']
})
export class RegisterAdminComponent {
  name = '';
  email = '';
  password = '';
  loading = false;
  errorMessage = '';
  successMessage = '';

  constructor(private authService: AuthService, private router: Router) {}

  register(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.name.trim() || !this.email.trim() || !this.password.trim()) {
      this.errorMessage = 'All fields are required.';
      return;
    }

    this.loading = true;

    this.authService.registerAdmin({
      name: this.name,
      email: this.email,
      password: this.password
    }).subscribe({
      next: () => {
        this.successMessage = 'Admin account created successfully!';
        this.loading = false;
        this.name = '';
        this.email = '';
        this.password = '';
      },
      error: (err) => {
        this.errorMessage = err.error?.message ?? 'Registration failed. Email may already exist.';
        this.loading = false;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/dashboard']);
  }
}