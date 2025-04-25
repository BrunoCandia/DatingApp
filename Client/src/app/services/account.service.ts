import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { User } from '../models/user';
import { map } from 'rxjs';
import { environment } from '../../environments/environment';
import { LikeService } from './like.service';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  ////baseUrl = 'https://localhost:7235/api/';
  baseUrl = environment.apiUrl;
  currentUser = signal<User | null>(null);

  constructor(private httpClient: HttpClient, private likeService: LikeService) {}

  login(model: any) {
    return this.httpClient
      .post<User>(this.baseUrl + 'account/login', model)
      .pipe(
        map((user) => {
          if (user) {
            this.setCurrentUser(user);
            // localStorage.setItem('user', JSON.stringify(user));
            // this.currentUser.set(user);
          }
        })
      );
  }

  register(model: any) {
    return this.httpClient
      .post<User>(this.baseUrl + 'account/register', model)
      .pipe(
        map((user) => {
          if (user) {
            this.setCurrentUser(user);
            // localStorage.setItem('user', JSON.stringify(user));
            // this.currentUser.set(user);
          }
          return user;
        })
      );
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }

  isLoggedIn() {
    return this.currentUser() !== null;
  }

  setCurrentUser(user: User) {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUser.set(user);
    this.likeService.getLikeIds();
  }
}
