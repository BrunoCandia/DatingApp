import { Component, OnInit } from '@angular/core';
import { MemberService } from '../../services/member.service';
import { Member } from '../../models/member';
import { MemberCardComponent } from "../member-card/member-card.component";
import { PaginatedResult } from '../../models/paginatedResult';
import { PageChangedEvent, PaginationComponent } from 'ngx-bootstrap/pagination';
import { AccountService } from '../../services/account.service';
import { User } from '../../models/user';
import { UserParams } from '../../models/userParams';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-member-list',
  imports: [MemberCardComponent, PaginationComponent, FormsModule],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit {

  members: Member[] = [];
  paginatedResult: PaginatedResult<Member[]> = new PaginatedResult<Member[]>();
  userParams: UserParams;
  genderList = [{ value: 'male', display: 'Males' }, { value: 'female', display: 'Females'}];

  constructor(private memberService: MemberService, private accountService: AccountService) {
    const user = this.accountService.currentUser();
    this.userParams = new UserParams(user);         // TODO: review this line
  }

  ngOnInit(): void {    
    this.loadMembers();
  }

  loadMembers() {    
    this.memberService.getMembers(this.userParams).subscribe({
      next: (response) => {
        this.paginatedResult.items = response.body as Member[];
        this.paginatedResult.pagination = response.headers.get('Pagination') ? JSON.parse(response.headers.get('Pagination')!) : null;
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  resetFilters() {
    this.userParams = new UserParams(this.accountService.currentUser()!);
    this.loadMembers();
  }

  pageChanged($event: PageChangedEvent) {
    if (this.userParams.pageNumber !== $event.page) {
      this.userParams.pageNumber = $event.page;
      this.loadMembers();
    }
  }
}
