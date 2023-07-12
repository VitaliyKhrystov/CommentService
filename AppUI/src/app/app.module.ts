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
import { LocalStorageService } from 'src/Services/local-storage.service';
import { AuthInterceptorService } from 'src/Services/auth-interceptor.service';
import { CommentComponent } from './components/comment/comment.component';
import { ReplyComponent } from './components/reply/reply.component';
import { ReplyFormComponent } from './components/reply/reply-form/reply-form.component';


@NgModule({
  declarations: [
    AppComponent,
    RegisterComponent,
    CommentFormComponent,
    LoginComponent,
    CommentComponent,
    ReplyComponent,
    ReplyFormComponent
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
    LocalStorageService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
