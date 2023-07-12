import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map, tap } from 'rxjs';
import { TokenModel } from 'src/app/Models/TokenModel';
import { AccountService } from './account.service';
import { LocalStorageService } from './local-storage.service';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptorService implements HttpInterceptor {

  constructor(private accountService: AccountService, private localStorage: LocalStorageService, private router:Router) { }

  tokenFromLocalStorage: TokenModel = {
    accessToken: '',
    refreshToken:''
  };
  newTokens: TokenModel = {
    accessToken: '',
    refreshToken:''
  };

  baseApiUrl: string = environment.apiURL;
  excludedURLs: string[] = environment.allowAnonymousURL;

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.tokenFromLocalStorage = JSON.parse(this.localStorage.getData("tokens"));
    if (this.tokenFromLocalStorage && !this.excludedURLs.includes(req.url)) {
      this.accountService.refreshToken(this.tokenFromLocalStorage).subscribe({
        next: response => {
          this.newTokens = response;
          window.localStorage.removeItem('tokens');
          this.localStorage.saveData("tokens", this.newTokens);
        },
        error: err => {
          this.localStorage.removeData("tokens");
          this.router.navigate(['login']);
          console.log(err);

        }
      });
      req = req.clone({
        setHeaders: {Authorization: `Bearer ${this.newTokens.accessToken}`}
      })
    }
    return next.handle(req)
    //   .pipe(
    //   tap({
    //     error: err => {
    //       if (err.status === 401) {
    //         console.log('User is not authenticated!')
    //         this.localStorage.removeData("tokens");
    //       }
    //     }
    // })
    // )

  }
}
