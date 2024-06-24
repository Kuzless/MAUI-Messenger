import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs';

@Component({
  selector: 'app-parameters',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './parameters.component.html',
  styleUrl: './parameters.component.css'
})
export class ParametersComponent implements OnInit {
  @Input() currentPage: any;
  @Input() columns!: string[];
  @Output() onSave: EventEmitter<void> = new EventEmitter<void>();

  parametersForm!: FormGroup;
  sortRule: { [key: string]: boolean } = {};

  constructor(private fb: FormBuilder) {
  }
  ngOnInit(): void {
    this.parametersForm = this.fb.group({
      substring: '',
      sort: this.columns[0],
      sortType: false
    })
    this.parametersForm.get('substring')?.valueChanges
      .pipe(
        debounceTime(1000),
        distinctUntilChanged()
      )
      .subscribe(() => {
        this.onSave.emit();
      })

    this.parametersForm.get('sort')?.valueChanges
      .subscribe(value => {
        this.currentPage.sort = { [value]: this.parametersForm.controls['sortType'].value }
        this.onSave.emit();
      })

    this.parametersForm.get('sortType')?.valueChanges
      .subscribe(value => {
        this.currentPage.sort = { [this.parametersForm.controls['sort'].value]: value }
        this.onSave.emit();
      })
  }
}
