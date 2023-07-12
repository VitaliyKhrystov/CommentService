import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RegisterModel } from 'src/app/Models/RegisterModel';
import { LoginModel } from 'src/app/Models/LoginModel';
import { TokenModel } from 'src/app/Models/TokenModel';
import { environment } from 'src/environments/environment';

const BaseApi = "https://localhost:7200/api";

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private http: HttpClient) { }

  baseApiUrl: string = environment.apiURL;

  getPing():Observable<string> {
    return this.http.get(this.baseApiUrl + "/api/Account/ping", {responseType:'text'});
  }

  register(registerModel: RegisterModel){
    return this.http.post(this.baseApiUrl + "/api/Account/register", registerModel, {responseType:'text'})
  }

  login(loginModel: LoginModel):Observable<TokenModel> {
    return this.http.post<TokenModel>(this.baseApiUrl + "/api/Account/login", loginModel, {responseType:'json'})
  }

  refreshToken(tokens: TokenModel):Observable<TokenModel> {
    return this.http.post<TokenModel>(this.baseApiUrl + "/api/Account/refresh-token", tokens, {responseType:'json'})
  }

  logout() {
    return this.http.post(this.baseApiUrl + "/api/Account/logout", "");
  }
}
