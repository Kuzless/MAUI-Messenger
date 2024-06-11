export class User {
    public name: string;
    public userName: string;
    public email: string;
    public phoneNumber: string;
  
    constructor(data?: Partial<User>) {
      this.name = '';
      this.userName = '';
      this.email = '';
      this.phoneNumber = '';
      if (data) {
        Object.assign(this, data);
      }
    }
}
