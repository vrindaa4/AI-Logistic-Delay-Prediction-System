import { HttpInterceptorFn } from '@angular/common/http';

export const authToken: HttpInterceptorFn =
(req, next) => {

  const token = localStorage.getItem('token');

  if (token) {
    req = req.clone({//Angular's HttpRequest objects are immutable.
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  return next(req);
};

//This interceptor automatically attaches the JWT token stored in localStorage to every outgoing HTTP request. It intercepts the request, checks whether a token exists, clones the immutable request object, adds an Authorization: Bearer <token> header, and forwards the modified request to the backend. This allows ASP.NET Core's JWT authentication middleware to validate the user and authorize access to protected endpoints without manually adding the token in every service call.