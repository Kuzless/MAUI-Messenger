export class DataRetrieval {
    sort: { [key: string]: boolean } = {};
    pageNumber: number = 1;
    pageSize: number = 10;
    subs: string = '';
  
    constructor(data?: Partial<Response>) {
      if (data) {
        Object.assign(this, data);
      }
    }
}
