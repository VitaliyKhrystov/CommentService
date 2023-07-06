import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-reply',
  templateUrl: './reply.component.html',
  styleUrls: ['./reply.component.css']
})
export class ReplyComponent {
  @Input() com: any;
  @Input() com2: any;

}
