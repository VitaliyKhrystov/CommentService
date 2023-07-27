import { Routes } from '@angular/router';

import { RegisterComponent } from './components/register/register.component';
import { CommentFormComponent } from './components/comment-form/comment-form.component';
import { LoginComponent } from './components/login/login.component';
import { CommentComponent } from './components/comment/comment.component';
import { ReplyComponent } from './components/reply/reply.component';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { authGuard } from 'src/Services/auth.guard';
import { NotFoundComponent } from './components/not-found/not-found.component';

export const appRouts: Routes = [
  { path: '', component: CommentComponent  },
  { path: 'comment-form', component: CommentFormComponent},
  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent },
  { path: 'reply', component: ReplyComponent },
  { path: 'reply-form', component: ReplyComponent },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  { path: 'reset-password', component: ResetPasswordComponent },
  { path: '**', component:NotFoundComponent, canActivate:[authGuard]}
]
