import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class WrapperService {

  private url = 'https://mymessengerapp.azurewebsites.net/api/';

  constructor(private http: HttpClient) {}

  async getAsync(urlEnd: string, token = ''): Promise<any> {
    const urlController = this.url + urlEnd;
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return firstValueFrom(this.http.get<any>(urlController, { headers }));
  }

  async postAsync(urlEnd: string, content: any, token = ''): Promise<any> {
    const urlController = this.url + urlEnd;
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return firstValueFrom(this.http.post<any>(urlController, content, { headers }));
  }

  async putAsync(urlEnd: string, content: any, token = ''): Promise<any> {
    const urlController = this.url + urlEnd;
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return firstValueFrom(this.http.put<any>(urlController, content, { headers }));
  }

  async deleteAsync(urlEnd: string, token = ''): Promise<any> {
    const urlController = this.url + urlEnd;
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return firstValueFrom(this.http.delete<any>(urlController, { headers }));
  }

  async postImageAsync(urlEnd: string, content: FormData, token = ''): Promise<any> {
    const urlController = this.url + urlEnd;
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return firstValueFrom(this.http.post<any>(urlController, content, { headers }));
  }
}
