import { HttpClient } from '@angular/common/http';
import { computed, Injectable, signal } from '@angular/core';
import { User } from '../models/user';
import { map } from 'rxjs';
import { environment } from '../../environments/environment';
import { LikeService } from './like.service';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  ////baseUrl = 'https://localhost:7235/api/';
  baseUrl = environment.apiUrl;
  currentUser = signal<User | null>(null);
  roles = computed(() => {
    const user = this.currentUser();
    if (user && user.token) {
      const role = JSON.parse(atob(user.token.split('.')[1])).role;
      return Array.isArray(role) ? role : [role];
    }

    return [];
  });

  constructor(private httpClient: HttpClient, private likeService: LikeService, private presenceService: PresenceService) {}

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
    this.presenceService.stopHubConnection();
  }

  isLoggedIn() {
    return this.currentUser() !== null;
  }

  setCurrentUser(user: User) {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUser.set(user);
    this.likeService.getLikeIds();
    this.presenceService.createHubConnection(user)
  }
}
