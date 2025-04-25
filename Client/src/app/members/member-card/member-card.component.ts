import { Component, computed, input } from '@angular/core';
import { Member } from '../../models/member';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { LikeService } from '../../services/like.service';

@Component({
  selector: 'app-member-card',
  imports: [CommonModule, RouterLink],
  templateUrl: './member-card.component.html',
  styleUrl: './member-card.component.css'
})
export class MemberCardComponent {

 member = input.required<Member>();

 hasLiked = computed<boolean>(() => this.likeService.likeIds().includes(this.member().userId));

 constructor(private likeService: LikeService) { }

 toggleLike() {
   this.likeService.toggleLike(this.member().userId).subscribe({
     next: () => {
       if (this.hasLiked()) {
          this.likeService.likeIds.update(ids => ids.filter(id => id !== this.member().userId));
       }
       else {
          this.likeService.likeIds.update(ids => [...ids, this.member().userId]);
       }
     },
     error: (error) => {
       console.error('Error toggling like:', error);
     }
   });
  }

}
