import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../services/auth.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  signUpAttempted: boolean = false;
  signUpSuccessfully: boolean = true;
  name: string = '';
  username: string = '';
  email: string = '';
  password: string = '';

  constructor(private authService: AuthService) {}

  async signUp() {
    this.authService.signUp(this.name, this.username, this.email, this.password)
      .subscribe(success => {
        this.signUpSuccessfully = success;
        this.signUpAttempted = true;
    });
  }
}