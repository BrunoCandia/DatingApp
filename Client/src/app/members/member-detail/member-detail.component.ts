import { Component, computed, OnDestroy, OnInit, signal, ViewChild } from '@angular/core';
import { MemberService } from '../../services/member.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Member } from '../../models/member';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TimeagoModule } from 'ngx-timeago';
import { DatePipe } from '@angular/common';
import { MemberMessageComponent } from "../member-message/member-message.component";
import { Message } from '../../models/message';
import { MessageService } from '../../services/message.service';
import { PresenceService } from '../../services/presence.service';
import { AccountService } from '../../services/account.service';
import { HubConnectionState } from '@microsoft/signalr';

@Component({
  selector: 'app-member-detail',
  imports: [TabsModule, GalleryModule, TimeagoModule, DatePipe, MemberMessageComponent],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css'
})
export class MemberDetailComponent implements OnInit, OnDestroy {
  @ViewChild('memberTabs', {static: true}) memberTabs?: TabsetComponent;
  activeTab?: TabDirective;
  //messages: Message[] = [];

  member: Member = {} as Member;
  images: GalleryItem[] = [];

  isOnline = computed<boolean>(() => this.presenceService.onlineUsers().includes(this.member.userName));

  constructor(
    private memberService: MemberService, 
    private route: ActivatedRoute, 
    private messageService: MessageService, 
    private presenceService: PresenceService, 
    private accountService: AccountService,
    private router: Router) { }  

  ngOnInit(): void {
    //this.loadMember();

    this.route.data.subscribe({
      next: data => {
        this.member = data['member'];
        this.member && this.member.photos.map(photo => {
          this.images.push(new ImageItem({src: photo.url, thumb: photo.url}));
        });
      },
      error: (error) => {
        console.error(error);
      }
    });

    this.route.paramMap.subscribe({
      next: _ => this.onRouteParamsChange()
    });

    this.route.queryParams.subscribe({
      next: params => {
        params['tab'] && this.selectTab(params['tab'])
      }
    });
  }

  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }

  loadMember() {
    const userName = this.route.snapshot.paramMap.get('userName');

    if (!userName) {
      console.error('Username not found in route parameters');
      return;
    }

    this.memberService.getMember(userName).subscribe({
      next: (response) => {
        this.member = response;        
          response.photos.map(photo => {
            this.images.push(new ImageItem({src: photo.url, thumb: photo.url}));
          });        
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  onRouteParamsChange() {
    const user = this.accountService.currentUser();

    if (!user) return;

    if (this.messageService.hubConnection?.state === HubConnectionState.Connected &&
        this.activeTab?.heading === 'Messages')
    {
      this.messageService.hubConnection
      .stop()
      .then(() => {this.messageService.createHubConnection(user, this.member.userName)})
    }
  }

  // This ensures that messages are only loaded when the "Messages" tab is activated, optimizing performance by avoiding unnecessary API calls.
  onTabActivated(data: TabDirective) {
    this.activeTab = data;

    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {tab: this.activeTab.heading},
      queryParamsHandling: 'merge'
    })
    
    this.tryLoadMessages();

    // // if (this.activeTab.heading === 'Messages' && this.messages.length === 0 && this.member) {
    // if (this.activeTab.heading === 'Messages' && this.messages.length === 0) {
    //   this.loadMessages();
    // }
  }

  private tryLoadMessages() {
    if (this.activeTab?.heading === 'Messages' && this.member) {
      const user = this.accountService.currentUser();
      
      if (!user) {
        console.log('No user found for loading messages');
        return;
      } 

      this.messageService.createHubConnection(user, this.member.userName);
    } else {
      this.messageService.stopHubConnection();
    }
  }

  // private loadMessages() {
  //   if (this.member) {
  //     this.messageService.getMessageThread(this.member.userName).subscribe({
  //       next: messages => this.messages = messages,
  //       error: (error) => {
  //         console.error(error);
  //       }
  //     })
  //   }
  // }

  selectTab(heading: string) {    
    if (this.memberTabs) {
      const messageTab = this.memberTabs?.tabs.find(x => x.heading == heading);

      if (messageTab) messageTab.active = true;
    }
  }

  // onUpdateMessages(event: Message) {
  //   this.messages.push(event);
  // }
}
