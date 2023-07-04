import { Component } from '@angular/core';
import { AccountService } from 'src/Services/account.service';
import { LoginModel } from 'src/app/Models/LoginModel';
import { Router } from '@angular/router';
import { TokenModel } from 'src/app/Models/TokenModel';
import { LoginModelErrors } from 'src/app/Models/LoginModelErrors';
import { LocalStorageService } from 'src/Services/local-storage.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  loginModel: LoginModel = {
    nickName: '',
    password: ''
  };


  tokenModel!: TokenModel;
  nickNameError: string = '';
  passwordError: string = '';
  errors!: LoginModelErrors;

  constructor(private accountService: AccountService, private router:Router, private localStorageService:LocalStorageService) { }

  login() {
    this.accountService.login(this.loginModel).
      subscribe({
        next: (response) => {
          console.log(response);
          this.tokenModel = response;
          this.localStorageService.saveData("tokens", this.tokenModel);
          this.router.navigate(["/"]);
        },
        error: (err) => {
          console.error(err);
          if (typeof err.error === "string") {
            this.errors = new LoginModelErrors();
            this.passwordError = '';
            this.nickNameError = err.error;
          } else if (typeof err.error.Password === "string") {
              this.errors = new LoginModelErrors();
              this.passwordError = err.error.Password
          } else {
              this.nickNameError = '';
              this.passwordError = '';
              this.errors = err.error.errors;
          }
        }
      });
  }
}
