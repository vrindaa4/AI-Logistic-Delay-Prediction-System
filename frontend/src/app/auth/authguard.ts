import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

 if (authService.isAdmin()) return true;
  return router.createUrlTree(['/shipments']); // redirect users to their own view
};

//Automatically attach the JWT token to every API request so that protected ASP.NET Core endpoints can identify the logged-in user.
//file defines an HTTP Interceptor in Angular.