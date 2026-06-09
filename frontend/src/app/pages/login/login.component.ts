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

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  login(): void {

    const data = {
      email: this.email,
      password: this.password
    };

    this.authService.login(data)
      .subscribe({
        next: (response: any) => {

          this.authService.saveToken(response.token);

          this.router.navigate(['/dashboard']);
        },

        error: (err) => {
          console.error(err);
          alert('Invalid credentials');
        }
      });
  }
}