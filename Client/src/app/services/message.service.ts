import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
} from '@microsoft/signalr';
import { environment } from '../../environments/environment';
import { Message } from '../models/message';
import { MessageParams } from '../models/messageParams';
import { PaginatedResult } from '../models/paginatedResult';
import { User } from '../models/user';
import { Group } from '../models/group';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  baseUrl = environment.apiUrl;
  hubUrl = environment.hubsUrl;
  hubConnection?: HubConnection;

  messageThread = signal<Message[]>([]);

  constructor(private httpClient: HttpClient) {}

  getMessages(messageParams: MessageParams) {
    let params = this.setPaginationHeader(
      messageParams.pageNumber,
      messageParams.pageSize
    );

    params = params.append('container', messageParams.container);

    return this.httpClient.get<PaginatedResult<Message[]>>(
      this.baseUrl + 'message',
      { observe: 'response', params }
    );
  }

  private setPaginationHeader(
    pageNumber: number,
    pageSize: number
  ): HttpParams {
    let params = new HttpParams();

    if (pageNumber && pageSize) {
      params = params.append('pageNumber', pageNumber.toString());
      params = params.append('pageSize', pageSize.toString());
    }

    return params;
  }

  getMessageThread(userName: string) {
    return this.httpClient.get<Message[]>(
      this.baseUrl + 'message/thread/' + userName
    );
  }

  sendMessage(userName: string, content: string) {
    return this.httpClient.post<Message>(this.baseUrl + 'message', {
      recipientUserName: userName,
      content,
    });
  }

  async sendMessageWhitSignalR(userName: string, content: string) {    
    return this.hubConnection?.invoke('SendMessage', {
      recipientUserName: userName,
      content,
    });    
  }

  deleteMessage(messageId: string) {
    return this.httpClient.delete(this.baseUrl + 'message/' + messageId);
  }

  createHubConnection(user: User, otherUserName: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'message?user=' + otherUserName, {
        accessTokenFactory: () => user.token,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch((error) => console.log(error));

    // this.hubConnection.start()
    //   .then(() => console.log("Connected to the message hub"))
    //   .catch(err => console.error("Error connecting to the hub:", err));

    this.hubConnection.on('ReceiveMessageThread', (messages: Message[]) => {
      console.log('message received');
      this.messageThread.set(messages);
    });

    this.hubConnection.on('NewMessage', (message: Message) => {
      console.log('new message received');
      this.messageThread.update(messages => [...messages, message]);
    });

    this.hubConnection.on('UpdatedGroup', (group: Group) => {
      console.log('group updated');
      ////this.messageThread.update(messages => [...messages, message]);
      if (group.connections.some(x => x.userName === otherUserName))
      {
        this.messageThread.update(messages => {
          messages.forEach(message => {
            if (!message.dateRead) {
              message.dateRead = new Date(Date.now());
            }
          })
          return messages;
        });
      }      
    });
  }

  stopHubConnection() {
    if (this.hubConnection?.state === HubConnectionState.Connected) {
      this.hubConnection.stop().catch((error) => console.log(error));
    }
  }
}
