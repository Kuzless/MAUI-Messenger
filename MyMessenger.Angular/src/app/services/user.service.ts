import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { DataGrid } from '../models/datagrid';
import { DataRetrieval } from '../models/dataretrieval';
import { WrapperService } from './wrapper.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient, private wrapper: WrapperService) { }

  async getAll<T>(data: DataRetrieval, endpoint: string): Promise<DataGrid<T>> {
    let queryString = `PageNumber=${data.pageNumber}&PageSize=${data.pageSize}`;

    if (data.subs) {
      queryString += `&Subs=${encodeURIComponent(data.subs)}`;
    }

    if (data.sort) {
      for (const [key, value] of Object.entries(data.sort)) {
        queryString += `&Sort[${encodeURIComponent(key)}]=${value}`;
      }
    }

    try {
      const accessToken = localStorage.getItem("accessToken") || undefined;
      const url = `${endpoint}?${queryString}`;
      return this.wrapper.getAsync(url, accessToken);
    } catch (error) {
      console.error('Error while fetching data:', error);
      return { data: [], numberOfPages: 1 };
    }
  }
}
