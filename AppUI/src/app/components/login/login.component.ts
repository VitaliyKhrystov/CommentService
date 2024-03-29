import { Component } from '@angular/core';
import { AccountService } from 'src/Services/account.service';
import { LoginModel } from 'src/app/Models/LoginModel';
import { Router } from '@angular/router';
import { TokenModel } from 'src/app/Models/TokenModel';
import { LoginModelErrors } from 'src/app/Models/LoginModelErrors';
import { AuthStorageService } from 'src/Services/auth-storage.service';

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
  errors: LoginModelErrors = {
    NickName: [] = [],
    Password: [] = []
  }

  constructor(private accountService: AccountService, private router:Router, private authStorage:AuthStorageService) { }

  login() {
    this.accountService.login(this.loginModel).
      subscribe({
        next: (response) => {
          this.tokenModel = response;
          this.authStorage.saveData("tokens", this.tokenModel);
          console.log(response);
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
              this.nickNameError = '';
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
