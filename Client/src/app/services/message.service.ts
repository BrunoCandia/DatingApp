import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MessageParams } from '../models/messageParams';
import { Message } from '../models/message';
import { PaginatedResult } from '../models/paginatedResult';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  baseUrl = environment.apiUrl

  constructor(private httpClient: HttpClient) { }

  getMessages(messageParams: MessageParams) {
    let params = this.setPaginationHeader(messageParams.pageNumber, messageParams.pageSize);

    params = params.append('container', messageParams.container);

    return this.httpClient.get<PaginatedResult<Message[]>>(this.baseUrl + 'message', { observe: 'response', params });
  }

  private setPaginationHeader(pageNumber: number, pageSize: number): HttpParams {
    let params = new HttpParams();

    if (pageNumber && pageSize) {
      params = params.append('pageNumber', pageNumber.toString());
      params = params.append('pageSize', pageSize.toString());
    }

    return params;
  }

  getMessageThread(userName: string) {
    return this.httpClient.get<Message[]>(this.baseUrl + 'message/thread/' + userName);
  }

  sendMessage(userName: string, content: string) {
    return this.httpClient.post<Message>(this.baseUrl + 'message', {recipientUserName: userName, content});
  }

  deleteMessage(messageId: string) {
    return this.httpClient.delete(this.baseUrl + 'message/' + messageId);
  }
}
