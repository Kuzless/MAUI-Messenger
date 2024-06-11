import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from '../../services/user.service';
import { AgGridModule } from 'ag-grid-angular'; 
import { User } from '../../models/user';
import { ColDef } from 'ag-grid-community';
import { DataRetrieval } from '../../models/dataretrieval';
import { DataGrid } from '../../models/datagrid';


@Component({
  selector: 'app-users',
  standalone: true,
  imports: [AgGridModule, CommonModule],
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})

export class UsersComponent {
  columnDefs: ColDef<User>[] = [
    { headerName: "Name", field: "name", sortable: false },
    { headerName: "UserName", field: "userName", sortable: false },
    { headerName: "Email", field: "email", sortable: false },
    { headerName: "Phone", field: "phoneNumber", sortable: false }
  ];
  isParametersVisible: boolean = false;
  pageSize: number = 10;
  usersList: User[] = [];
  currentPage: DataRetrieval;
  isWindowVisible = false;
  numberOfPages = 1;
  currentPageNumber = 1;

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
    this.usersList.forEach(element => {
      console.log(element  );
    });
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
