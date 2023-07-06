import { Injectable } from '@angular/core';
import jwt_decode from "jwt-decode";
import { TokenModel } from 'src/app/Models/TokenModel';
import { LocalStorageService } from './local-storage.service';

@Injectable({
  providedIn: 'root'
})
export class JwtService {

  constructor(private localStorage: LocalStorageService) { }

  tokens: TokenModel = {
    accessToken: '',
    refreshToken:''
  }

  decodeJWT(): any {
    this.tokens = JSON.parse(this.localStorage.getData('tokens'));
    return jwt_decode(this.tokens.accessToken);
  }
}
