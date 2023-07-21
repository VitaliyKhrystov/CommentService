import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from 'src/Services/account.service';
import { DataTransferService } from 'src/Services/data-transfer.service';
import { ForgotPassword } from 'src/app/Models/ForgotPassword';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent {

  constructor(private router:Router, private accountService: AccountService, private dataTransfer: DataTransferService) { }

  email: string = '';
  notify: string = 'Please enter your email address. You will receive a number that allow you to reset your password.';
  errorMessage: string = '';
  isError: boolean = false;

  forgot() {
    this.accountService.forgotPassword(this.email).subscribe({
      next: forgotPassword => {
        this.dataTransfer.setData(forgotPassword);
        this.isError = false;
        this.router.navigate(['reset-password']);
      },
      error: err => {
        console.log(err);
        if (err.error.errors) {
          this.errorMessage = err.error.errors.Email[0];
        } else {
          this.errorMessage = err.error;
        }
        this.isError = true;
      }
    });
  }

}
