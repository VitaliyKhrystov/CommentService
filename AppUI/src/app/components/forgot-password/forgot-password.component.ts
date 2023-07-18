import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent {

  constructor(private router:Router) { }

  email: string = '';
  notify: string = 'Please enter your email address. You will receive a number that allow you to reset your password.';

  forgotPassword() {

  this.router.navigate(['reset-password']);
  }

}
