import { Component, OnInit } from '@angular/core';
import { CommentService } from 'src/Services/comment.service';
import { JwtService } from 'src/Services/jwt.service';
import { CommentModelResponse, CommenttModelRequest } from 'src/app/Models/CommentModel';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.css']
})
export class CommentComponent implements OnInit{

    commentRequest: CommenttModelRequest = {
    commentText: '',
    parrentId: '',
    topicURL: 'http://test.com',
    userId: ''
  }

  comments: CommentModelResponse[] = [];

  constructor(private commentService: CommentService, private jwtService: JwtService) { }

  ngOnInit(): void {
    this.getAllComments();
    this.commentService.RefreshData.subscribe(res => {
      this.getAllComments();
    })
  }

  ngDoCheck() {

  }

  createComment(e: any) {
    this.commentRequest.commentText = e;
    this.commentRequest.userId = this.jwtService.decodeJWT().nameid;
    this.commentService.createComment(this.commentRequest).subscribe({
      next: res => {
        // this.getAllComments()
        console.log(res);
      },
      error: err => {
        console.error(err);
      }
    });
  }

  getAllComments() {
    this.commentService.getAllComments().subscribe({
      next: res => {
        this.comments = res as CommentModelResponse[];
        this.comments = this.comments.sort((a, b) => (a.createdAt > b.createdAt) ? -1 : 1);
        console.log(this.comments);
      }
    })

  }

}
