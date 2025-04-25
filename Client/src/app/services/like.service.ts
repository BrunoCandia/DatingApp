import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../models/member';
import { PaginatedResult } from '../models/paginatedResult';
import { LikeParams } from '../models/likeParams';

@Injectable({
  providedIn: 'root',
})
export class LikeService {
  baseUrl = environment.apiUrl;
  likeIds = signal<string[]>([]);

  constructor(private httpClient: HttpClient) {}

  getLikes(likeParams: LikeParams) {
    let params = this.setPaginationHeader(likeParams.pageNumber, likeParams.pageSize);
    
    params = params.append('predicate', likeParams.predicate);

    //return this.httpClient.get<PaginatedResult<Member[]>>(this.baseUrl + 'userlike?predicate=' + likeParams.predicate, { observe: 'response', params });

    return this.httpClient.get<PaginatedResult<Member[]>>(this.baseUrl + 'userlike', { observe: 'response', params });
  }

  private setPaginationHeader(pageNumber: number, pageSize: number): HttpParams {
    let params = new HttpParams();

    if (pageNumber && pageSize) {
      params = params.append('pageNumber', pageNumber.toString());
      params = params.append('pageSize', pageSize.toString());
    }

    return params;
  }

  toggleLike(targetUserId: string) {
    return this.httpClient.post(this.baseUrl + 'userlike/' + targetUserId, {});
  }

  // getLikes(predicate: string) {
  //   return this.httpClient.get<Member[]>(this.baseUrl + 'userlike?predicate=' + predicate);
  // }

  getLikeIds() {
    return this.httpClient.get<string[]>(this.baseUrl + 'userlike/list').subscribe({
      next: (ids) => {
        this.likeIds.set(ids);
      },
      error: (error) => {
        console.error('Error fetching like IDs:', error);
      }
    });
  }

}
