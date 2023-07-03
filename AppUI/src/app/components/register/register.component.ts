import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from 'src/Services/account.service';
import { RegisterModel } from 'src/app/Models/RegisterModel';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  registerModel: RegisterModel =
    {
      nickName: "",
      email: "",
      password: "",
      birthYear: 0,
      roleName:2
    }

  errors!: string;

  constructor(private accountService: AccountService, private router:Router) { }

  register() {
    this.accountService.register(this.registerModel)
      .subscribe({
         next: (response) => {
          console.log(response);
          this.router.navigate(["login"]);
        },
        error: (err:HttpErrorResponse) => {
          this.errors = err.error.substring(1, err.error.indexOf('}'));
          console.log(this.errors);
             console.error(err);
           }
      })
  }
}

interface JSONData{
  Email: string[],
  NickName:string[],
  Password:string[],
  BirthYear:string[]
}
