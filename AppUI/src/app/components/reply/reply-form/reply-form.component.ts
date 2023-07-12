import { Component, Input, OnInit } from '@angular/core';
import { CommentComponent } from '../../comment/comment.component';
import { CommentService } from 'src/Services/comment.service';

@Component({
  selector: 'app-reply-form',
  templateUrl: './reply-form.component.html',
  styleUrls: ['./reply-form.component.css']
})
export class ReplyFormComponent implements OnInit {

  constructor(private commentComponent: CommentComponent, private commentService: CommentService) { }

  ngOnInit(): void {}

  @Input() comment: any;

  isReply: boolean = false;
  isHidden: boolean = false;
  isEdit: boolean = false;
  row: number = 1;

  getRows(text: string): number {
    let num = text.length / 35;
    return this.row = num < this.row ?  this.row : Math.ceil(num);
  }

  edit() {
    this.isEdit = true;
  
  }
}
