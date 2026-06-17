import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  name = '';
  email = '';
  password = '';
  loading = false;
  errorMessage = '';
  successMessage = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  register(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.name.trim() || !this.email.trim() || !this.password.trim()) {
      this.errorMessage = 'All fields are required.';
      return;
    }

    this.loading = true;

    const data = {
      name: this.name,
      email: this.email,
      password: this.password
    };

    this.authService.register(data).subscribe({
      next: () => {
        this.successMessage = 'Account created! Redirecting to login...';
        this.loading = false;
        setTimeout(() => this.router.navigate(['/login']), 1500);
      },
      error: (err) => {
        this.errorMessage = err.error ?? 'Registration failed. Email may already exist.';
        this.loading = false;
      }
    });
  }
}