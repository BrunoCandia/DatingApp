import { Component, OnInit } from '@angular/core';
import { LikeService } from '../services/like.service';
import { Member } from '../models/member';
import { FormsModule } from '@angular/forms';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { MemberCardComponent } from '../members/member-card/member-card.component';
import { PaginatedResult } from '../models/paginatedResult';
import { LikeParams } from '../models/likeParams';
import { PageChangedEvent, PaginationComponent } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-lists',
  imports: [FormsModule, ButtonsModule, MemberCardComponent, PaginationComponent],
  templateUrl: './lists.component.html',
  styleUrl: './lists.component.css'
})
export class ListsComponent implements OnInit {

  predicate = 'liked';

  paginatedResult: PaginatedResult<Member[]> = new PaginatedResult<Member[]>();
  likeParams: LikeParams;

  constructor(private likeService: LikeService) {
    this.likeParams = new LikeParams();         // Initialize likeParams with default values
    this.likeParams.predicate = this.predicate; // Set the predicate to 'liked' or 'likedBy'
  }
  
  ngOnInit(): void {
    this.loadLikes();
  }

  loadLikes() {
    this.likeService.getLikes(this.likeParams).subscribe({
      next: (response) => {
        this.paginatedResult.items = response.body as Member[];
        this.paginatedResult.pagination = response.headers.get('Pagination') ? JSON.parse(response.headers.get('Pagination')!) : null;
      },
      error: (error) => {
        console.error('Error fetching likes:', error);
      }
    });
  }

  getTitle() {
    switch (this.predicate) {
      case 'liked': return 'Members you liked';
      case 'likedBy': return 'Members who liked you';
      default: return 'Mutual Likes';
    }
  }

  pageChanged($event: PageChangedEvent) {
      if (this.likeParams.pageNumber !== $event.page) {
        this.likeParams.pageNumber = $event.page;
        this.loadLikes();
      }
    }
}
