import { Injectable } from '@angular/core';
import { Observable, of, BehaviorSubject, map, catchError } from 'rxjs';
import { Response } from '../interfaces/response';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = environment.baseUrl;
  private loggedIn = new BehaviorSubject<boolean>(this.isLogged());
  isLoggedIn = this.loggedIn.asObservable();

  constructor(private http: HttpClient) { }

  login(email: string, password: string): Observable<boolean> {
    const data = { Email: email, Password: password };
    return this.http.post(this.baseUrl + 'Auth/', data)
      .pipe(
        map(
        tokens => {
          this.handleLoginResponse(tokens);
          return true;
    }),
    catchError((error: HttpErrorResponse) => {
      return of(false);
    }))
  }
  signUp(name: string, username: string, email: string, password: string): Observable<boolean> {
    const data = { Name: name, UserName: username, Email: email, Password: password };
    return this.http.post<Response>(this.baseUrl + 'Auth/sign', data)
      .pipe(
        map(response => {
          return response.isSuccessful;
        }),
        catchError((error: HttpErrorResponse) => {
          return of(false);
        }))
  }

  isLogged(): boolean {
    return typeof window !== 'undefined' && !!localStorage.getItem('accessToken');
  }

  private handleLoginResponse(response: any): void {
    localStorage.setItem('accessToken', response.accessToken);
    localStorage.setItem('refreshToken', response.refreshToken);
    this.loggedIn.next(true);
  }
  
  logout() {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    this.loggedIn.next(false);
  }
}
