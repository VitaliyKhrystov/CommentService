import { Component, Input, OnInit } from '@angular/core';
import { CommentComponent } from '../../comment/comment.component';
import { CommentService } from 'src/Services/comment.service';
import { UpdateCommentModel } from 'src/app/Models/UpdateCommentModel';
import { JwtService } from 'src/Services/jwt.service';

@Component({
  selector: 'app-reply-form',
  templateUrl: './reply-form.component.html',
  styleUrls: ['./reply-form.component.css']
})
export class ReplyFormComponent implements OnInit {

  constructor(private commentComponent: CommentComponent, private commentService: CommentService, private jwtService: JwtService) { }

  ngOnInit(): void {
    this.currentMessage = this.comment.commentText;
  }

  @Input() comment: any;

  isReply: boolean = false;
  isHidden: boolean = false;
  isEdit: boolean = false;
  isDelete: boolean = true;
  row: number = 1;
  currentMessage!: string;
  updateComment: UpdateCommentModel = {
    commentId: '',
    commentText: '',
    userId: ''
  }

  getRows(text: string): number {
    let num = text.length / 35;
    return this.row = num < this.row ?  this.row : Math.ceil(num);
  }

  update() {
    this.isEdit = true;
    this.updateComment.commentId = this.comment.commentId;
    this.updateComment.userId = this.comment.userId;
    this.updateComment.commentText = this.comment.commentText;
    console.log(this.updateComment);
    this.commentService.updateComment(this.updateComment).subscribe({
      next: res => {
        console.log(res);
      },
      error: err => {
        console.log(err);
        this.comment.commentText = this.currentMessage;
      }
    });
    this.isEdit = false;
  }

  delete(choice: string) {
    if (choice === 'yes') {
      this.commentService.deleteComment(this.comment.commentId).subscribe({
      next: res => {
        console.log();
      },
      error: err => {
        console.log(err);
      }
    })
    }
    this.isDelete = true;
  }

  isCanEditOrDelete(): boolean {
    let token = this.jwtService.decodeJWT();
    if (token) {
      let userID = token["nameid"];
      let role = token["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
      if (userID) {
        if (this.comment.userId == userID || role == 'Admin') {
          return true;
        }
      }
   }
    return false;
  }

  setLike() {

  }
}
