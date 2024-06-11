export class Response {
    public isSuccessful: boolean;
    public message: string;
  
    constructor(data?: Partial<Response>) {
      this.isSuccessful = false;
      this.message = '';
      if (data) {
        Object.assign(this, data);
      }
    }
}
