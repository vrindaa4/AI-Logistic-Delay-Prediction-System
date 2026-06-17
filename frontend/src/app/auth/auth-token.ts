import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

export const authToken: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem('token');
  const router = inject(Router);

  if (token) {
    req = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });
  }

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        localStorage.removeItem('token');
        localStorage.removeItem('role');
        router.navigate(['/login']);
      }
      return throwError(() => error);
    })
  );
};
//This interceptor automatically attaches the JWT token stored in localStorage to every outgoing HTTP request. It intercepts the request, checks whether a token exists, clones the immutable request object, adds an Authorization: Bearer <token> header, and forwards the modified request to the backend. This allows ASP.NET Core's JWT authentication middleware to validate the user and authorize access to protected endpoints without manually adding the token in every service call.