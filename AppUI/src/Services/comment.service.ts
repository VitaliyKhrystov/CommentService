import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject, tap } from 'rxjs';
import { CommenttModelRequest } from 'src/app/Models/CommentModel';
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
    return this.http.post(baseApiURL + "/api/Comment/create", comment, { responseType: 'text' }).pipe(
      tap(() => {
        this.RefreshData.next(true);
      })
    )
  }

  getAllComments(){
    return this.http.get(baseApiURL + "/api/Comment/getAllComments", { responseType: 'json' });
  }


}
