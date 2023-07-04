import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {
  [x: string]: any;

  constructor() { }

  IsAuthozised(key:string):boolean {
    if (this.getData(key) != null) {
      return true;
    } else {
      return false;
    }
  }

  saveData(key: string, value: object): void{
    this.removeData(key);
    return localStorage.setItem(key, JSON.stringify(value).toString());
  }

  getData(key: string): any{
    return localStorage.getItem(key);
  }

  removeData(key: string) {
    localStorage.removeItem(key);
  }

  clearData() {
    localStorage.clear();
  }
}
