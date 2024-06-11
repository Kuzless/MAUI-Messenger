export class DataGrid<T> {
    data!: T[];
    numberOfPages: number;
  
    constructor(init?: Partial<DataGrid<T>>) {
      this.numberOfPages = 0;
      Object.assign(this, init);
    }
  }
  