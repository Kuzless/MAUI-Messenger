import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subscription } from 'rxjs';

import { AgGridModule } from 'ag-grid-angular';
import { ColDef, SizeColumnsToFitGridStrategy } from 'ag-grid-community';

import { User } from './interfaces/user';
import { DataRetrieval } from '../../shared/interfaces/dataretrieval';

import { UserService } from './services/user.service';

import { ParametersComponent } from '../../shared/components/parameters/parameters.component';

import { PermissionsCheckDirective } from '../../directives/permissions-check.directive';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [
    AgGridModule,
    CommonModule,
    ParametersComponent,
    PermissionsCheckDirective,
  ],
  templateUrl: './users.component.html',
  styleUrl: './users.component.css',
})
export class UsersComponent implements OnInit {
  @ViewChild(ParametersComponent) parametersComponent!: ParametersComponent;
  columns: string[] = ['Name', 'UserName', 'Email', 'PhoneNumber'];
  autoSizeStrategy: SizeColumnsToFitGridStrategy = {
    type: 'fitGridWidth',
    defaultMinWidth: 100,
  };
  defaultColDef = {
    resizable: false,
    sortable: false,
    suppressMovable: true,
  };
  columnDefs: ColDef<User>[] = [
    { headerName: this.columns[0], field: 'name' },
    { headerName: this.columns[1], field: 'userName' },
    { headerName: this.columns[2], field: 'email' },
    {
      headerName: this.columns[3],
      field: 'phoneNumber',
      cellRenderer: function (params: any) {
        return `<a href="tel:${params.value}">${params.value}</a>`;
      },
    },
    {
      headerName: '',
      cellRenderer: function () {
        return '<img src="assets/call.png"/>';
      },
    },
  ];
  isParametersVisible: boolean = false;
  pageSize: number = 10;
  usersList: User[] = [];
  currentPage: DataRetrieval = new DataRetrieval();
  isWindowVisible = false;
  numberOfPages = 1;
  currentPageNumber = 1;
  onSave!: Subscription;

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.currentPage = {
      pageNumber: this.currentPageNumber,
      pageSize: this.pageSize,
      sort: { Name: false },
      subs: '',
    };
    this.getAllUsers();
  }

  changePage(): void {
    this.currentPage.pageNumber = this.currentPageNumber;
    this.getAllUsers();
  }

  getAllUsers(): void {
    this.userService
      .getAll<User>(this.currentPage, 'User')
      .subscribe((data) => {
        this.usersList = data.data;
        this.numberOfPages = data.numberOfPages;
      });
  }

  changePopupVisibility(): void {
    this.isParametersVisible = !this.isParametersVisible;
  }
}
