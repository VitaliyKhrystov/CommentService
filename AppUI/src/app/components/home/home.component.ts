import { Component, DoCheck, EventEmitter, Output } from '@angular/core';
import { AccountService } from 'src/Services/account.service';
import { LocalStorageService } from 'src/Services/local-storage.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {

  constructor(private accontService: AccountService, private localStorage: LocalStorageService) { }

  isAuthorized!: boolean;
  comment: string = '';
  @Output() commentText: EventEmitter<string> = new EventEmitter();

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

  sendMessage() {
    console.log(this.comment);
    this.commentText.emit(this.comment);
    this.comment = '';
  }
}
