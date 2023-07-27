import { Component, EventEmitter, OnInit, Output, Input, inject } from '@angular/core';
import { AccountService } from 'src/Services/account.service';
import { CommentService } from 'src/Services/comment.service';
import { JwtService } from 'src/Services/jwt.service';
import { AuthStorageService } from 'src/Services/auth-storage.service';
import { CommenttModelRequest } from 'src/app/Models/CommentModel';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-comment-form',
  templateUrl: './comment-form.component.html',
  styleUrls: ['./comment-form.component.css']
})
export class CommentFormComponent implements OnInit{

  constructor(private accontService: AccountService, private authStorage: AuthStorageService, private jwtService: JwtService, private commentService: CommentService){ }

  ngOnInit(): void {
    if (this.authStorage.getData('tokens')) {
      this.isAuthorized = true;
    }
    this.authStorage.IsAuthorized.subscribe({
      next: res => {
        this.isAuthorized = res as boolean;
        console.log(res);
      }
    });
  }

  @Input() parrentId: string = '';
  isAuthorized: boolean = false;
  topicUrl = environment.topicURL;
    commentRequest: CommenttModelRequest = {
      commentText: '',
      parrentId: '',
      topicURL: this.topicUrl,
      userId: ''
  }

  logout() {
    this.accontService.logout().subscribe({
      next: res => {
        console.log(res);
        this.authStorage.removeData("tokens");
      },
      error: err => {
        console.log(err);
        this.authStorage.removeData("tokens");
      }
    });
  }

  sendMessage() {
    this.commentRequest.userId = this.jwtService.decodeJWT().nameid;
    this.commentRequest.parrentId = this.parrentId;
    this.commentService.createComment(this.commentRequest)
      .subscribe({
      next: res => {
        console.log(res);
      },
      error: err => {
        console.error(err);
      }
    });
    this.commentRequest.commentText = '';
  }
}

