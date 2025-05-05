import { Component, OnInit } from '@angular/core';
import { PageChangedEvent, PaginationComponent } from 'ngx-bootstrap/pagination';
import { Message } from '../models/message';
import { MessageParams } from '../models/messageParams';
import { PaginatedResult } from '../models/paginatedResult';
import { MessageService } from '../services/message.service';
import { FormsModule } from '@angular/forms';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { TimeagoModule } from 'ngx-timeago';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-messages',
  imports: [PaginationComponent, FormsModule, ButtonsModule, TimeagoModule, RouterLink],
  templateUrl: './messages.component.html',
  styleUrl: './messages.component.css',
})
export class MessagesComponent implements OnInit {
  paginatedResult: PaginatedResult<Message[]> = new PaginatedResult<Message[]>();
  messageParams: MessageParams;
  isOutbox = false;

  constructor(private messageService: MessageService) {
    this.messageParams = new MessageParams();
    this.isOutbox = this.messageParams.container === 'Outbox';
  }

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages() {
    this.messageService.getMessages(this.messageParams).subscribe({
      next: (response) => {
        this.paginatedResult.items = response.body as Message[];
        this.paginatedResult.pagination = response.headers.get('Pagination')
          ? JSON.parse(response.headers.get('Pagination')!)
          : null;
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  pageChanged($event: PageChangedEvent) {
    if (this.messageParams.pageNumber !== $event.page) {
      this.messageParams.pageNumber = $event.page;
      this.loadMessages();
    }
  }

  getRoute(message: Message) {
    if (this.messageParams.container === 'Outbox') {
      return `/members/${message.recipientUserName}`;
    } else {
      return `/members/${message.senderUserName}`
    }
  }

  deleteMessage(messageId: string) {
    this.messageService.deleteMessage(messageId).subscribe({
      next: () => {
        // Remove the deleted message from the local array
        this.paginatedResult.items = this.paginatedResult.items?.filter(
          (message) => message.messageId !== messageId
        );
        console.log(`Message with ID ${messageId} deleted successfully.`);
      },
      error: (error) => {
        console.error(`Failed to delete message with ID ${messageId}:`, error);
      },
    })
  }
}
