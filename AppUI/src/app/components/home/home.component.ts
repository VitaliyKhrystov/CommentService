import { Component, DoCheck } from '@angular/core';
import { AccountService } from 'src/Services/account.service';
import { LocalStorageService } from 'src/Services/local-storage.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {

  constructor(private accontService: AccountService, private localStorage: LocalStorageService) { }

  isAuthorized!:boolean;

  logout() {
    this.accontService.logout().subscribe({
      next: res => {
        console.log(res);
        this.localStorage.removeData('tokens');
      },
      error: err => {
        console.error(err);
      }
    });
  }

  ngDoCheck() {
    this.isAuthorized = this.localStorage.IsAuthozised('tokens');
  }

}
