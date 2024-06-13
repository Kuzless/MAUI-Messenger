import { Component, Input, Output, EventEmitter, SimpleChanges, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-parameters',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './parameters.component.html',
  styleUrl: './parameters.component.css'
})
export class ParametersComponent implements OnChanges {
  @Input() currentPage: any;
  @Input() columns!: string[];
  @Output() onOpen: EventEmitter<void> = new EventEmitter<void>();
  @Output() onClose: EventEmitter<void> = new EventEmitter<void>();
  @Output() onSave: EventEmitter<void> = new EventEmitter<void>();

  sortType: boolean;
  selectedColumn!: string;
  sort: { [key: string]: boolean } = {};

  constructor() {
    this.sortType = false;
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.columns = changes['columns'].currentValue;
    this.selectedColumn = this.columns[0];
    this.sort[this.columns[0]] = this.sortType;
  }

  saveAndClose() {
    this.currentPage.sort = this.sort;
    this.onSave.emit();
  }

  sortByColumn(event: any) {
    const column = event.target.value;
    this.selectedColumn = column;
    this.sort = { [this.selectedColumn]: this.sortType };
  }
  
  changeSortType() {
    this.sortType = !this.sortType;
    this.sort[this.selectedColumn] = this.sortType;
  }
}
