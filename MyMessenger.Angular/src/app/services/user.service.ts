import { Injectable } from '@angular/core';
import { DataGrid } from '../shared/models/datagrid';
import { DataRetrieval } from '../shared/models/dataretrieval';
import { WrapperService } from './wrapper.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private wrapper: WrapperService) { }

  async getAll<T>(data: DataRetrieval, endpoint: string): Promise<DataGrid<T>> {
    try {
      const accessToken = localStorage.getItem("accessToken") || undefined;
      const url = this.buildUrl(data, endpoint);
      return this.wrapper.getAsync(url, accessToken);
    } catch (error) {
      console.error('Error while fetching data:', error);
      return { data: [], numberOfPages: 1 };
    }
  }
  buildUrl(data: DataRetrieval, endpoint: string) : string {
    let queryString = `PageNumber=${data.pageNumber}&PageSize=${data.pageSize}`;
    if (data.subs) {
      queryString += `&Subs=${encodeURIComponent(data.subs)}`;
    }
    if (data.sort) {
      for (const [key, value] of Object.entries(data.sort)) {
        queryString += `&Sort[${encodeURIComponent(key)}]=${value}`;
      }
    }
    return `${endpoint}?${queryString}`;
  }
}
