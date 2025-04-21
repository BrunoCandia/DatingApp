import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../models/member';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class MemberService {

  baseUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient, private accountService: AccountService) {}

  getMembers() {
    return this.httpClient.get<Member[]>(this.baseUrl + 'user');
  }

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
