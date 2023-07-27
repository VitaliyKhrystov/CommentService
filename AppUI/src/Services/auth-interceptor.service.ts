import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map, switchMap, tap, throwError } from 'rxjs';
import { TokenModel } from 'src/app/Models/TokenModel';
import { AccountService } from './account.service';
import { AuthStorageService } from './auth-storage.service';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptorService implements HttpInterceptor {

  constructor(private accountService: AccountService, private authStorage: AuthStorageService, private router:Router) { }

  excludedURLs: string[] = environment.allowAnonymousURL;

  intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const tokensFromLocalStorage: TokenModel = JSON.parse(this.authStorage.getData("tokens"));
    let request = req;
    if (tokensFromLocalStorage  && !this.excludedURLs.includes(req.url)) {
      request = req.clone({
        setHeaders: {Authorization:`Bearer ${tokensFromLocalStorage.accessToken}`}
      })
    }
    return next.handle(request).pipe(
      catchError((err: any) => {
        if (err instanceof HttpErrorResponse) {
          if (err.status === 401) {
            return this.handleUnAuthorizedError(req, next);
          }
        }
        return throwError(() => err);
      }));
  }

  handleUnAuthorizedError(request:HttpRequest<any>, next:HttpHandler) {
    let tokens: TokenModel = JSON.parse(this.authStorage.getData("tokens"));
    return this.accountService.refreshToken(tokens).pipe(
      switchMap((data: TokenModel) => {
        this.authStorage.saveData("tokens", data);
        const newRequest = request.clone({
          setHeaders: { Authorization: `Bearer ${data.accessToken}` }
        })
        return next.handle(newRequest);
      }),
      catchError((err) => {
        console.log(err);
        if (err.error === "Invalid access token or refresh token") {
          this.authStorage.removeData("tokens");
        }
        return throwError(() => err)
      })
    )
  }
}
