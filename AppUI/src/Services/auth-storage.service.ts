import { Injectable, OnInit } from '@angular/core';
import {Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthStorageService implements OnInit {

  constructor() { }

  ngOnInit(): void {
    if (this.getData("tokens")) {
      this.isAuthozised.next(true);
    } else {
      this.isAuthozised.next(false);
    }
  }

  private isAuthozised: Subject<boolean> = new Subject<boolean>();
  get IsAuthorized(){
    return this.isAuthozised;
  }


  saveData(key: string, value: object): void{
    localStorage.setItem(key, JSON.stringify(value).toString());
    this.isAuthozised.next(true);
  }

  getData(key: string): any{
    return localStorage.getItem(key);
  }

  removeData(key: string) {
    localStorage.removeItem(key);
    this.isAuthozised.next(false);
  }

  clearData() {
    localStorage.clear();
  }
}
