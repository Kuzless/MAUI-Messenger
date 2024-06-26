import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../service/auth.service';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  signUpAttempted: boolean = false;
  registerForm!: FormGroup;

  constructor(private authService: AuthService, private fb: FormBuilder, private router: Router) {}
  
  ngOnInit(): void {
    this.registerForm = new FormGroup({});
    this.registerForm = this.fb.group({
      name: ['', [Validators.required]],
      username: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.pattern('^(?=.*[A-Z].*[A-Z])(?=.*[a-z].*[a-z])(?=.*\\d.*\\d)(?=.*[\\W_].*[\\W_])[A-Za-z\\d\\W_]{8,}$')]]
    })
  }

  isFormInvalid(controlName: string): boolean {
    const control = this.registerForm.controls[controlName];
    
    const result = control.invalid && control.touched;
    
    return result;
  }

  async signUp() {
    this.authService.signUp(this.registerForm.controls['name'].value, this.registerForm.controls['username'].value, this.registerForm.controls['email'].value, this.registerForm.controls['password'].value)
      .subscribe(success => {
        this.signUpAttempted = true;
        if (success) 
        {
          alert('Sign up successful!');
          this.router.navigate(['login']);
        }
    });
  }
}