import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ForgotPassword } from 'src/app/Models/ForgotPassword';

@Injectable({
  providedIn: 'root'
})
export class DataTransferService {

  constructor() { }

  private initial: ForgotPassword = {
    userId: '',
    confirmationNumber: 0
  };

  private forgotPassport = new BehaviorSubject(this.initial);
  getData = this.forgotPassport.asObservable();

  setData(data: ForgotPassword) {
    this.forgotPassport.next(data);
  }
}
