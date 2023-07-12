import { Injectable } from '@angular/core';
import { Subject, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {

  constructor() {
    this.IsAuthozised.next(this.getData('tokens'));
   }

  IsAuthozised: Subject<boolean> = new Subject<boolean>();

  saveData(key: string, value: object): void{
    this.IsAuthozised.next(true);
    localStorage.setItem(key, JSON.stringify(value).toString());
  }

  getData(key: string): any{
    return localStorage.getItem(key);
  }

  removeData(key: string) {
    localStorage.removeItem(key);
    this.IsAuthozised.next(false);
  }

  clearData() {
    localStorage.clear();
  }
}
