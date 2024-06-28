import { Routes } from '@angular/router';
import { LoginComponent } from './pages/auth/login/login.component';
import { RegisterComponent } from './pages/auth/register/register.component'; 
import { UsersComponent } from './pages/users/users.component';
import { UnauthorizedGuard } from './services/unauthorized.guard';
import { AuthorizedGuard } from './services/authorized.guard';


export const routes: Routes = [
    { path: '', redirectTo: 'login', pathMatch: 'full' },
    { path: 'login', component: LoginComponent, canActivate: [AuthorizedGuard] },
    { path: 'register', component: RegisterComponent, canActivate: [AuthorizedGuard]},
    { path: 'users', component: UsersComponent, canActivate: [UnauthorizedGuard]},
    { path: '**', redirectTo: 'users' }
];
