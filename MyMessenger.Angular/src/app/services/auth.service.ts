import { Injectable } from '@angular/core';
import { WrapperService } from './wrapper.service';
import { Observable, of, BehaviorSubject } from 'rxjs';
import { Response } from './models/response';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private loggedIn = new BehaviorSubject<boolean>(this.isLogged());
  isLoggedIn = this.loggedIn.asObservable();

  constructor(private wrapper: WrapperService) { }

  async login(email: string, password: string): Promise<boolean> {
    const data = { Email: email, Password: password };
    try{
      const response = await this.wrapper.postAsync('Auth/', data);
      this.handleLoginResponse(response);
      return true;
    } catch {
      console.error('Invalid login credentials.');
      return false;
    }
  }
  async signUp(name: string, username: string, email: string, password: string): Promise<Observable<boolean>> {
      const data = { Name: name, UserName: username, Email: email, Password: password };
      const response = await this.wrapper.postAsync('Auth/sign', data);
      const responseModel : Response = new Response(response)
      return of(responseModel.isSuccessful);
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
