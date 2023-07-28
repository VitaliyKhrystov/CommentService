import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { appRouts } from './app.routs';
import { AppComponent } from './app.component';
import { RegisterComponent } from './components/register/register.component';
import { CommentFormComponent } from './components/comment-form/comment-form.component';
import { LoginComponent } from './components/login/login.component';
import { AccountService } from 'src/Services/account.service';
import { AuthStorageService } from 'src/Services/auth-storage.service';
import { AuthInterceptorService } from 'src/Services/auth-interceptor.service';
import { CommentComponent } from './components/comment/comment.component';
import { ReplyComponent } from './components/reply/reply.component';
import { ReplyFormComponent } from './components/reply/reply-form/reply-form.component';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { CommentService } from 'src/Services/comment.service';
import { DataTransferService } from 'src/Services/data-transfer.service';
import { JwtService } from 'src/Services/jwt.service';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { ContactsComponent } from './components/contacts/contacts.component';


@NgModule({
  declarations: [
    AppComponent,
    RegisterComponent,
    CommentFormComponent,
    LoginComponent,
    CommentComponent,
    ReplyComponent,
    ReplyFormComponent,
    ForgotPasswordComponent,
    ResetPasswordComponent,
    NotFoundComponent,
    ContactsComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    RouterModule.forRoot(appRouts)
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptorService,
      multi: true
    },
    AccountService,
    //The same{{provide:AccountService, useClass:AccountService}}
    AuthStorageService,
    CommentService,
    DataTransferService,
    JwtService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
