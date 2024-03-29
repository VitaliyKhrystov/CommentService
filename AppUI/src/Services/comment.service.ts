import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject, tap } from 'rxjs';
import { CommenttModelRequest, DisLike, Like } from 'src/app/Models/CommentModel';
import { UpdateCommentModel } from 'src/app/Models/UpdateCommentModel';
import { environment } from 'src/environments/environment';

const baseApiURL = environment.apiURL;

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  constructor(private http: HttpClient) { }

  private refreshData = new Subject<boolean>();

  get RefreshData() {
    return this.refreshData;
  }

  createComment(comment: CommenttModelRequest) {
    this.RefreshData.next(true);
    return this.http.post(baseApiURL + "/api/Comment/create", comment, { responseType: 'text' }).pipe(
      tap(() => {
        this.RefreshData.next(true);
      })
    )
  }

  getAllComments(){
    return this.http.get(baseApiURL + "/api/Comment/getAllComments", { responseType: 'json' });
  }

  updateComment(model: UpdateCommentModel) {
    return this.http.put(baseApiURL + "/api/Comment/update", model).pipe(
      tap(() => {
        this.RefreshData.next(true);
      })
    );
  }

  deleteComment(commentId: string) {
    return this.http.delete(baseApiURL + "/api/Comment/delete/" + commentId).pipe(
      tap(() => {
        this.RefreshData.next(true);
      })
    );
  }

  setLike(like: Like) {
    return this.http.post(baseApiURL + "/api/Comment/setLike", like).pipe(
      tap(() => {
        this.RefreshData.next(true);
      })
    );;
  }

  deleteLike(like: Like) {
    return this.http.post(baseApiURL + "/api/Comment/deleteLike", like).pipe(
      tap(() => {
        this.RefreshData.next(true);
      })
    );
    }

  setDisLike(disLike: DisLike) {
    return this.http.post(baseApiURL + "/api/Comment/setDisLike", disLike).pipe(
      tap(() => {
        this.RefreshData.next(true);
      })
    );
   }

  deleteDisLike(disLike: DisLike) {
    return this.http.post(baseApiURL + "/api/Comment/deleteDisLike", disLike).pipe(
      tap(() => {
        this.RefreshData.next(true);
      })
    );
  }
}
