import { Component, OnInit } from '@angular/core';
import { MemberService } from '../../services/member.service';
import { Member } from '../../models/member';
import { MemberCardComponent } from "../member-card/member-card.component";
import { PaginatedResult } from '../../models/paginatedResult';
import { PageChangedEvent, PaginationComponent } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-member-list',
  imports: [MemberCardComponent, PaginationComponent],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit {

  members: Member[] = [];
  paginatedResult: PaginatedResult<Member[]> = new PaginatedResult<Member[]>();
  pageNumber = 1;
  pageSize = 5;

  constructor(private memberService: MemberService) {}

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    this.memberService.getMembers(this.pageNumber, this.pageSize).subscribe({
      next: (response) => {
        this.paginatedResult.items = response.body as Member[];
        this.paginatedResult.pagination = response.headers.get('Pagination') ? JSON.parse(response.headers.get('Pagination')!) : null;
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  pageChanged($event: PageChangedEvent) {
    if (this.pageNumber !== $event.page) {
      this.pageNumber = $event.page;
      this.loadMembers();
    }
  }
}
