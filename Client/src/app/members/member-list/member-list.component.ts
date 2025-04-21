import { Component, OnInit } from '@angular/core';
import { MemberService } from '../../services/member.service';
import { Member } from '../../models/member';
import { MemberCardComponent } from "../member-card/member-card.component";

@Component({
  selector: 'app-member-list',
  imports: [MemberCardComponent],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit {
  members: Member[] = [];

  constructor(private memberService: MemberService) {}

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    this.memberService.getMembers().subscribe({
      next: (response) => {
        this.members = response;
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

}
