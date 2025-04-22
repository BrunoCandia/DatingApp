import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../models/member';
import { AccountService } from './account.service';
import { PaginatedResult } from '../models/paginatedResult';
import { User } from '../models/user';
import { UserParams } from '../models/userParams';

@Injectable({
  providedIn: 'root'
})
export class MemberService {

  baseUrl = environment.apiUrl;  

  constructor(private httpClient: HttpClient, private accountService: AccountService) {}

  getMembers(userParams: UserParams) {
    let params = this.setPaginationHeader(userParams.pageNumber, userParams.pageSize);

    params = params.append('minAge', userParams.minAge.toString());
    params = params.append('maxAge', userParams.maxAge.toString());
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);

    return this.httpClient.get<PaginatedResult<Member[]>>(this.baseUrl + 'user', { observe: 'response', params });
  }

  private setPaginationHeader(pageNumber: number, pageSize: number): HttpParams {
    let params = new HttpParams();

    if (pageNumber && pageSize) {
      params = params.append('pageNumber', pageNumber.toString());
      params = params.append('pageSize', pageSize.toString());
    }

    return params;
  }

  // getMembers(pageNumber?: number, pageSize?: number) {
  //   let params = new HttpParams();

  //   if (pageNumber && pageSize) {
  //     params = params.append('pageNumber', pageNumber.toString());
  //     params = params.append('pageSize', pageSize.toString());
  //   }

  //   return this.httpClient.get<PaginatedResult<Member[]>>(this.baseUrl + 'user', { observe: 'response', params });
  // }

  // getMembers() {
  //   return this.httpClient.get<Member[]>(this.baseUrl + 'user');
  // }

  getMember(userName: string) {
    return this.httpClient.get<Member>(this.baseUrl + 'user/' + userName);
  }

  updateMember(member: Member) {
    return this.httpClient.put<Member>(this.baseUrl + 'user', member);
  }

  // getMembers() {
  //   return this.httpClient.get<Member[]>(this.baseUrl + 'user', this.getHttpOptions());
  // }

  // getMember(userName: string) {
  //   return this.httpClient.get<Member>(this.baseUrl + 'user/' + userName, this.getHttpOptions());
  // }

  // getHttpOptions() {
  //   return {
  //     headers: new HttpHeaders({
  //       // Authorization: 'Bearer ' + localStorage.getItem('token') || '',
  //       Authorization: `Bearer ${this.accountService.currentUser()?.token}`
  //     })
  //   };
  // }
}
