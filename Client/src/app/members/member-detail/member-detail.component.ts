import { Component, OnInit } from '@angular/core';
import { MemberService } from '../../services/member.service';
import { ActivatedRoute } from '@angular/router';
import { Member } from '../../models/member';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';

@Component({
  selector: 'app-member-detail',
  imports: [TabsModule, GalleryModule],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css'
})
export class MemberDetailComponent implements OnInit {

  member?: Member;
  images: GalleryItem[] = [];

  constructor(private memberService: MemberService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadMember();
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
}
