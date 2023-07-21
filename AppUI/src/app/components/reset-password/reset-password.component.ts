import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from 'src/Services/account.service';
import { DataTransferService } from 'src/Services/data-transfer.service';
import { ForgotPassword } from 'src/app/Models/ForgotPassword';
import { ResetPassword } from 'src/app/Models/ResetPassword';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {

  constructor(private account: AccountService, private router: Router, private dataTrasfer: DataTransferService) { }

  ngOnInit(): void {
    this.dataTrasfer.getData.subscribe({
      next: res => {
        this.forgotPassword = res;
      }
    });
  }

  confirmationNumber: number = 0;
  password: string = '';
  errorMessage: string = '"Incorrect confirmation data!"';
  isError: boolean = false;
  resetPassword: ResetPassword = {
    userId: '',
    confirmationNumber: 0,
    password: ''
  }

  forgotPassword!: ForgotPassword;

  reset() {
    this.resetPassword.confirmationNumber = this.confirmationNumber;
    this.resetPassword.userId = this.forgotPassword.userId;
    this.resetPassword.password = this.password;
    this.account.resetPassword(this.resetPassword).subscribe({
      next: res => {
        this.isError = false;
        this.router.navigate(['login']);
      },
      error: err => {
        this.isError = true;
        console.log(err);
      }
    })

  }

}
