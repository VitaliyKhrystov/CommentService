import { Injectable } from '@angular/core';
import jwt_decode from "jwt-decode";
import { TokenModel } from 'src/app/Models/TokenModel';
import { AuthStorageService } from './auth-storage.service';

@Injectable({
  providedIn: 'root'
})
export class JwtService {

  constructor(private authStorage: AuthStorageService) { }

  tokens: TokenModel = {
    accessToken: '',
    refreshToken:''
  }

  decodeJWT(): any {
    this.tokens = JSON.parse(this.authStorage.getData('tokens'));
    if (this.tokens) {
      return jwt_decode(this.tokens.accessToken);
    }
    return null;
  }
}
