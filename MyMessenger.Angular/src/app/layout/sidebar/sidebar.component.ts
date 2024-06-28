import { Component } from '@angular/core';
import { AuthService } from '../../pages/auth/service/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
  isLoggedIn!: boolean;

  constructor(private authService: AuthService) 
  {
    this.authService.isLoggedIn.subscribe((loggedIn: boolean) => {
      this.isLoggedIn = loggedIn;
    });
  }
  
  logout() {
    this.authService.logout();
  }
}
