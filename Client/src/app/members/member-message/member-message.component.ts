import { Component, computed, input, OnInit, output, signal, ViewChild } from '@angular/core';
import { Message } from '../../models/message';
import { MessageService } from '../../services/message.service';
import { TimeagoModule } from 'ngx-timeago';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-message',
  imports: [TimeagoModule, FormsModule],
  templateUrl: './member-message.component.html',
  styleUrl: './member-message.component.css'
})
export class MemberMessageComponent implements OnInit {
  @ViewChild('messageForm') messageForm?: NgForm;

  userName = input.required<string>();
  // messages = input.required<Message[]>();
  // updateMessages = output<Message>();
  messageContent = '';

  //messages: Message[] = [];

  //messages = signal<Message[]>([]);
  messages = computed<Message[]>(() => this.messageService.messageThread());

  constructor(private messageService: MessageService) {}

  ngOnInit(): void {
    //Back-end call moved to member-detail.component, so the call is executed of the Message tab is activated
    //this.loadMessages();
    
    //this.messages.set(this.messageService.messageThread());
  }

  // loadMessages() {
  //   this.messageService.getMessageThread(this.userName()).subscribe({
  //     next: messages => this.messages = messages,
  //     error: (error) => {
  //       console.error(error);
  //     }
  //   })
  // }

  sendMessage() {
    //With SignalR
    this.messageService.sendMessageWhitSignalR(this.userName(), this.messageContent)
    .then(() => { this.messageForm?.reset();})
    .catch((error) => {console.log(error)});

    //With Http
    // this.messageService.sendMessage(this.userName(), this.messageContent).subscribe({
    //   next: (message) => {
    //     //this.updateMessages.emit(message);
    //     this.messageForm?.reset();
    //   },
    //   error: (error) => {
    //     console.log(error);
    //   }
    // });
  }
}
