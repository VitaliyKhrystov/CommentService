import { Component, EventEmitter, OnInit, Output, Input } from '@angular/core';
import { AccountService } from 'src/Services/account.service';
import { CommentService } from 'src/Services/comment.service';
import { JwtService } from 'src/Services/jwt.service';
import { LocalStorageService } from 'src/Services/local-storage.service';
import { CommenttModelRequest } from 'src/app/Models/CommentModel';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-comment-form',
  templateUrl: './comment-form.component.html',
  styleUrls: ['./comment-form.component.css']
})
export class CommentFormComponent implements OnInit{

  constructor(private accontService: AccountService, private localStorage: LocalStorageService, private jwtService: JwtService, private commentService: CommentService){ }

  ngOnInit(): void {
    this.isAuthorized = this.localStorage.getData('tokens');
    this.localStorage.IsAuthozised.subscribe({
      next: res => {
        this.isAuthorized = res;
      }
    });
  }

  @Input() parrentId: string = '';
  isAuthorized!: boolean;
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
        this.localStorage.removeData('tokens');
        console.log(res);
      },
      error: err => {
        this.localStorage.removeData('tokens');
        console.error(err);
      }
    });
  }

  sendMessage() {
    this.commentRequest.userId = this.jwtService.decodeJWT().nameid;
    this.commentRequest.parrentId = this.parrentId;
    this.commentService.createComment(this.commentRequest).subscribe({
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

