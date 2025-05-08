import { Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { User } from '../models/user';
import { Router } from '@angular/router';
import { take } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {

  hubUrl = environment.hubsUrl;
  private hubConnection?: HubConnection;
  onlineUsers = signal<string[]>([]);

  constructor(private toastrService: ToastrService, private router: Router) { }

  createHubConnection(user: User) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'presence', {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(error => console.log(error));
    
    this.hubConnection.on('UserIsOnline', userName => {
      this.onlineUsers.update(users => [...users, userName]);
      //this.toastrService.info(userName + ' has connected');
    });

    this.hubConnection.on('UserIsOffline', userName => {
      this.onlineUsers.update(users => users.filter(x => x !== userName));
      //this.toastrService.warning(userName + ' has disconnected');
    });

    this.hubConnection.on('GetOnlineUsers', userNames => {
      this.onlineUsers.set(userNames);
    });

    this.hubConnection.on('NewMessageReceived', ({userName, knownAs}) => {
      this.toastrService.info(knownAs + ' has sent oyu a new message, Click me to see it')
        .onTap
        .pipe(take(1))
        .subscribe(() => this.router.navigateByUrl('/members/' + userName + '?tab=Messages'));
    });
  }

  stopHubConnection() {
    if (this.hubConnection?.state === HubConnectionState.Connected) {
      this.hubConnection.stop().catch(error => console.log(error));
    }
  }
}
