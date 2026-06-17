import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router, RouterLink } from '@angular/router';

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
    private router: Router
  ) {}

  selectRole(role: 'User' | 'Admin'): void {
    this.selectedRole = role;
    this.errorMessage = '';
  }

  login(): void {
    this.errorMessage = '';
    this.loading = true;

    const data = {
      email: this.email,
      password: this.password
    };

    this.authService.login(data)
      .subscribe({
        next: (response: any) => {
          this.loading = false;

          this.authService.saveToken(response.token);
          this.authService.saveRole(response.role);

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