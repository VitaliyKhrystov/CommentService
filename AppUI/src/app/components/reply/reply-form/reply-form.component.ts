import { Component, Input, OnInit } from '@angular/core';
import { CommentService } from 'src/Services/comment.service';
import { UpdateCommentModel } from 'src/app/Models/UpdateCommentModel';
import { JwtService } from 'src/Services/jwt.service';
import { DisLike, Like } from 'src/app/Models/CommentModel';
import { AuthStorageService } from 'src/Services/auth-storage.service';

@Component({
  selector: 'app-reply-form',
  templateUrl: './reply-form.component.html',
  styleUrls: ['./reply-form.component.css']
})
export class ReplyFormComponent implements OnInit {

  constructor(
    private commentService: CommentService,
    private jwtService: JwtService,
    private authStorage: AuthStorageService) { }

  ngOnInit(): void {
    this.currentMessage = this.comment.commentText;
    this.token = this.jwtService.decodeJWT();
    this.isAuthorized = this.authStorage.getData("tokens") ? true : false;
    this.authStorage.IsAuthorized.subscribe({
      next: res => {
        this.isAuthorized = res ? true : false;
      }
    });
    if (this.token) {
      this.userId = this.token["nameid"];
      this.role = this.token["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
      if (this.comment.disLikes.find((dislike: DisLike) => { return dislike.userId === this.userId }) != null) {
        this.imgDislikeInvert = false;
      } else {
         this.imgDislikeInvert = true;
      }
      if (this.comment.likes.find((like: Like) => { return like.userId === this.userId }) != null) {
        this.imgLikeInvert = false;
      } else {
         this.imgLikeInvert = true;
      }
    }
  }

  @Input() comment: any;

  token:any;
  userId!: string;
  role!: string;
  isReply: boolean = false;
  isHidden: boolean = false;
  isEdit: boolean = false;
  isDelete: boolean = true;
  isAuthorized!: boolean;
  imgLikeInvert!: boolean;
  imgDislikeInvert!: boolean;
  row: number = 1;
  currentMessage: string = '';

  updateComment: UpdateCommentModel = {
    commentId: '',
    commentText: '',
    userId: ''
  }

  like: Like = {
    commentId: '',
    userId: ''
  }

   disLike: DisLike = {
    commentId: '',
    userId: ''
  }

  canReply() {
    this.isReply = !this.isReply;
    this.isHidden = false;
  }

  showReply() {
    this.isHidden = !this.isHidden;
    this.isReply = false;
  }

  getRows(text: string): number {
    let num = text.length / 35;
    return this.row = num < this.row ?  this.row : Math.ceil(num);
  }

  update() {
    this.isEdit = true;
    this.updateComment.commentId = this.comment.commentId;
    this.updateComment.userId = this.comment.userId;
    this.updateComment.commentText = this.currentMessage;
    console.log(this.updateComment);
    this.commentService.updateComment(this.updateComment).subscribe({
      next: res => {
        console.log(res);
      },
      error: err => {
        console.log(err);
      }
    });
    this.isEdit = false;
  }

  cancel() {
    this.isEdit = !this.isEdit;
    this.currentMessage = this.comment.commentText;
  }

  delete(choice: string) {
    if (choice === 'yes') {
      this.commentService.deleteComment(this.comment.commentId).subscribe({
      next: res => {
        console.log(res);
      },
      error: err => {
        console.log(err);
      }
    })
    }
    this.isDelete = true;
  }

  isCanEditOrDelete(): boolean {
    if (this.userId) {
      if ((this.comment.userId == this.userId || this.role == 'Admin' || this.role == 'Moderator') && this.isAuthorized) {
        return true;
      }
   }
    return false;
  }

  isCanLikeDisLike(): boolean{
    if (this.userId != this.comment.userId) {
      return true;
    }
    return false;
  }

  setLike() {
    if (this.userId && this.comment.commentId)
    {
      this.like.userId = this.userId;
      this.like.commentId = this.comment.commentId;
      if (!(this.comment.likes.find((like:any) => { return like.userId === this.userId }) != null ||
        this.comment.disLikes.find((dislike:any) => {return dislike.userId === this.userId}) !=null
      ))
      {
        this.commentService.setLike(this.like).subscribe({
          next: res => {
            this.imgLikeInvert = true;
          },
          error: err => {
            console.log(err);
          }
        });
      }
      else if (this.comment.likes.find((like: any) => { return like.userId === this.userId }) != null)
      {
        this.commentService.deleteLike(this.like).subscribe({
          next: res => {
            this.imgLikeInvert = false;
          },
          error: err => {
            console.log(err);
          }
        })
      }
    }
  }

  setDisLike() {
    if (this.userId && this.comment.commentId)
    {
      this.disLike.userId = this.userId;
      this.disLike.commentId = this.comment.commentId;
      if (!(this.comment.likes.find((like:any) => { return like.userId === this.userId }) != null ||
        this.comment.disLikes.find((dislike:any) => {return dislike.userId === this.userId}) !=null
      ))
      {
        this.commentService.setDisLike(this.disLike).subscribe({
          next: res => {
            this.imgDislikeInvert = true;
          },
          error: err => {
            console.log(err);
          }
        });
      }
      else if (this.comment.disLikes.find((dislike: any) => { return dislike.userId === this.userId }) != null)
      {
        this.commentService.deleteDisLike(this.disLike).subscribe({
          next: res => {
            this.imgDislikeInvert = false;
          },
          error: err => {
            console.log(err);
          }
        })
      }
    }
  }
}
