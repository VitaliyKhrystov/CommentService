import { Component } from '@angular/core';
import { AccountService } from 'src/Services/account.service'

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  test: any = "Test message";

  constructor(private accountClient:AccountService) { }

  getPingFromWebApi() {
       this.accountClient.getPing()
         .subscribe({
           next: (response) => {
             console.log(response);
             this.test = response;
           },
           error: (err) => {
             console.error(err);
           }
           }
    );
  }
}
