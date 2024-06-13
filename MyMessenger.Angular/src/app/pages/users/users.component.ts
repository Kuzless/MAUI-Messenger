import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from '../../services/user.service';
import { AgGridModule } from 'ag-grid-angular'; 
import { User } from '../../models/user';
import { ColDef } from 'ag-grid-community';
import { DataRetrieval } from '../../models/dataretrieval';
import { DataGrid } from '../../models/datagrid';
import { Subscription } from 'rxjs';
import { ParametersComponent } from '../../shared/parameters/parameters.component';
import { ViewChild } from '@angular/core';


@Component({
  selector: 'app-users',
  standalone: true,
  imports: [AgGridModule, CommonModule, ParametersComponent],
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})

export class UsersComponent {
  @ViewChild(ParametersComponent) parametersComponent!: ParametersComponent;
  columns: string[] = ["Name", "UserName", "Email", "Phone"];
  columnDefs: ColDef<User>[] = [
    { headerName: this.columns[0], field: "name", sortable: false },
    { headerName: this.columns[1], field: "userName", sortable: false },
    { headerName: this.columns[2], field: "email", sortable: false },
    { headerName: this.columns[3], field: "phoneNumber", sortable: false }
  ];
  isParametersVisible: boolean = false;
  pageSize: number = 10;
  usersList: User[] = [];
  currentPage: DataRetrieval;
  isWindowVisible = false;
  numberOfPages = 1;
  currentPageNumber = 1;
  onOpen!: Subscription;
  onSave!: Subscription;
  onClose!: Subscription;

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
    this.isParametersVisible = false;
  }

  openPopup(): void {
    this.isParametersVisible = true;
  }

  closePopup(): void {
    this.isParametersVisible = false;
  }
}
