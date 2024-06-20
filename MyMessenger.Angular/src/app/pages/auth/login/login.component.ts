import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  loginSuccessfully = true;

  constructor(private authService: AuthService, private router: Router, private fb: FormBuilder) {}

  ngOnInit(): void {
    this.loginForm = new FormGroup({});
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    })
  }
  isFormInvalid(controlName: string): boolean {
    const control = this.loginForm.controls[controlName];
    
    const result = control.invalid && control.touched;
    
    return result;
  }
  login(): void {
    this.authService.login(this.loginForm.controls['email'].value, this.loginForm.controls['password'].value).subscribe(bool => {
      if(bool) {
        this.router.navigate(['users'])
      } else {
        this.loginSuccessfully = bool;
      }
    })
  }
}
