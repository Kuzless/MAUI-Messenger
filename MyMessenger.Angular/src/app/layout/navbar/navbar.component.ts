import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ZoomDirective } from '../../directives/zoom.directive';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, ZoomDirective],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent {
  toggleSidebar() {
    const sidebar = document.querySelector('#sidebar');
    if (sidebar) {
      sidebar.classList.toggle('active');
    }
  }
}
