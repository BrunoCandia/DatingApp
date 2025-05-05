import { Component, OnInit, ViewChild } from '@angular/core';
import { MemberService } from '../../services/member.service';
import { ActivatedRoute } from '@angular/router';
import { Member } from '../../models/member';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TimeagoModule } from 'ngx-timeago';
import { DatePipe } from '@angular/common';
import { MemberMessageComponent } from "../member-message/member-message.component";
import { Message } from '../../models/message';
import { MessageService } from '../../services/message.service';

@Component({
  selector: 'app-member-detail',
  imports: [TabsModule, GalleryModule, TimeagoModule, DatePipe, MemberMessageComponent],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css'
})
export class MemberDetailComponent implements OnInit {
  @ViewChild('memberTabs', {static: true}) memberTabs?: TabsetComponent;
  activeTab?: TabDirective;
  messages: Message[] = [];

  member: Member = {} as Member;
  images: GalleryItem[] = [];

  constructor(private memberService: MemberService, private route: ActivatedRoute, private messageService: MessageService) { }

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

    this.route.queryParams.subscribe({
      next: params => {
        params['tab'] && this.selectTab(params['tab'])
      }
    });
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

  // This ensures that messages are only loaded when the "Messages" tab is activated, optimizing performance by avoiding unnecessary API calls.
  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    
    // if (this.activeTab.heading === 'Messages' && this.messages.length === 0 && this.member) {
    if (this.activeTab.heading === 'Messages' && this.messages.length === 0) {
      this.loadMessages();
    }
  }

  private loadMessages() {
    if (this.member) {
      this.messageService.getMessageThread(this.member.userName).subscribe({
        next: messages => this.messages = messages,
        error: (error) => {
          console.error(error);
        }
      })
    }
  }

  selectTab(heading: string) {    
    if (this.memberTabs) {
      const messageTab = this.memberTabs?.tabs.find(x => x.heading == heading);

      if (messageTab) messageTab.active = true;
    }
  }

  onUpdateMessages(event: Message) {
    this.messages.push(event);
  }
}
