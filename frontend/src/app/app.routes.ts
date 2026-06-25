import { Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { ShipmentListComponent } from './components/shipment-list/shipment-list.component';
import { PredictionComponent } from './components/prediction/prediction.component';
import { LoginComponent } from './pages/login/login.component';
import { authGuard } from './auth/authguard';
import { RegisterComponent } from './pages/register/register.component';
import { RegisterAdminComponent } from './pages/register-admin/register-admin.component';
import { adminGuard } from './auth/adminguard';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },  
  { path: 'dashboard', component: DashboardComponent, canActivate: [authGuard] },
  { path: 'shipments', component: ShipmentListComponent, canActivate: [authGuard] },
  { path: 'predict', component: PredictionComponent, canActivate: [authGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'register-admin', component: RegisterAdminComponent, canActivate: [adminGuard] },
  { path: '**', redirectTo: '/login' }
];