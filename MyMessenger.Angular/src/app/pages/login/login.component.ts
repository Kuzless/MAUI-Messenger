import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  loginSuccessfully: boolean = true;

  constructor(private authService: AuthService, private router: Router) {}

  async login(): Promise<void> {
    if (this.email && this.password) {
      this.loginSuccessfully = await this.authService.login(this.email, this.password);
      if (this.loginSuccessfully) {
        this.router.navigate(['users'])
      }
    }
  }
}
