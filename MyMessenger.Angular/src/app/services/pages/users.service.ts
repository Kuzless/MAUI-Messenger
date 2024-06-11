import { Injectable } from '@angular/core';
import { UserService } from '../user.service';
import { User } from '../../models/user';
import { DataRetrieval } from '../../models/dataretrieval';
import { DataGrid } from '../../models/datagrid';

@Injectable({
  providedIn: 'root'
})
export class UserPageService {
  usersList: User[] = [];
  currentPage: DataRetrieval;
  columns: string[] = ['Name', 'UserName', 'Email', 'PhoneNumber'];
  isWindowVisible = false;
  numberOfPages = 1;
  currentPageNumber = 1;
  pageSize = 10;

  constructor(private userService: UserService) {
    this.currentPage = {
      pageNumber: this.currentPageNumber,
      pageSize: this.pageSize,
      sort: { Name: false },
      subs: ''
    };
    this.getAllUsers();
  }

  changePage(): void {
    this.currentPage.pageNumber = this.currentPageNumber;
    this.getAllUsers();
  }

  async getAllUsers(): Promise<void> {
    const newData: DataGrid<User> = await this.userService.getAll(this.currentPage, 'User');
    this.usersList = newData.data;
    this.numberOfPages = newData.numberOfPages;
  }

  // Parameters
  saveChanges(): void {
    this.getAllUsers();
  }

  openPopup(): void {
    this.isWindowVisible = true;
  }

  closePopup(): void {
    this.isWindowVisible = false;
  }
}
