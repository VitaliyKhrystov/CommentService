import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RegisterModel } from 'src/app/Models/RegisterModel';
import { LoginModel } from 'src/app/Models/LoginModel';
import { TokenModel } from 'src/app/Models/TokenModel';


@Injectable({
  providedIn: 'root'
})
export class AccountService {

  resp!: any;
  constructor(private http: HttpClient) { }

  getPing():Observable<string> {

    return this.http.get("https://localhost:7200/api/Account/ping", {responseType:'text'});
  }

  register(registerModel: RegisterModel){
    return this.http.post("https://localhost:7200/api/Account/register", registerModel, {responseType:'text'})
  }

  login(loginModel: LoginModel):Observable<TokenModel> {
    return this.http.post<TokenModel>("https://localhost:7200/api/Account/login", loginModel, {responseType:'json'})
  }
}
