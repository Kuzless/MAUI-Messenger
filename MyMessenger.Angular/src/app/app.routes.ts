import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component'; 
import { UsersComponent } from './pages/users/users.component';
import { AuthorizedGuardService } from './services/authorized_guard.service';
import { UnauthorizedGuardService } from './services/unauthorized-guard.service';

export const routes: Routes = [
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: 'login', component: LoginComponent, canActivate: [UnauthorizedGuardService] },
    { path: 'register', component: RegisterComponent, canActivate: [UnauthorizedGuardService]},
    { path: 'users', component: UsersComponent, canActivate: [AuthorizedGuardService]}
];
