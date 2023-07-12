import { Component, OnInit } from '@angular/core';
import { CommentService } from 'src/Services/comment.service';
import { JwtService } from 'src/Services/jwt.service';
import { CommentModelResponse, CommenttModelRequest } from 'src/app/Models/CommentModel';
import { CommentMessageParrentId } from 'src/app/Models/CommentMessageParrentId';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.css']
})
export class CommentComponent implements OnInit{

  comments: CommentModelResponse[] = [];

  constructor(private commentService: CommentService, private jwtService: JwtService) { }

  ngOnInit(): void {
    this.getAllComments();
    this.commentService.RefreshData.subscribe(res =>
    { this.getAllComments(); });
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
