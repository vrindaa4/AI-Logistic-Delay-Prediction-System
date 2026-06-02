import { Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { ShipmentListComponent } from './components/shipment-list/shipment-list.component';
import { PredictionComponent } from './components/prediction/prediction.component';

export const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'shipments', component: ShipmentListComponent },
  { path: 'predict', component: PredictionComponent },
  { path: '**', redirectTo: '/dashboard' }
];
