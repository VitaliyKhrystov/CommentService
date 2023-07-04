import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TokenModel } from 'src/app/Models/TokenModel';
import { AccountService } from './account.service';
import { LocalStorageService } from './local-storage.service';
import { environment } from 'src/environments/environment';

@Injectable()
export class AuthInterceptorService implements HttpInterceptor {

  constructor(private accountService: AccountService, private localStorage: LocalStorageService) { }

  tokenFromLocalStorage!: TokenModel;
  newTokens!: TokenModel;
  baseApiUrl: string = environment.apiURL;

  excludedURLs: string[] = [
    this.baseApiUrl + '/api/Account/register',
    this.baseApiUrl + '/api/Account/login',
    this.baseApiUrl + '/api/Account/refresh-token'
  ];

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.tokenFromLocalStorage = JSON.parse(this.localStorage.getData("tokens"));
    if (this.tokenFromLocalStorage && !this.excludedURLs.includes(req.url)) {
      this.accountService.refreshToken(JSON.parse(this.localStorage.getData("tokens"))).subscribe({
        next: response => {
          this.newTokens = response;
          this.localStorage.saveData("tokens", this.newTokens);
        },
        error: err => {
          console.error(err);
        }
      });
      req = req.clone({
        setHeaders: {Authorization: `Bearer ${this.newTokens.accessToken}`}
      })
    }
    return next.handle(req);
  }
}
