import { Component, input } from '@angular/core';
import { Member } from '../../models/member';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-member-card',
  imports: [CommonModule, RouterLink],
  templateUrl: './member-card.component.html',
  styleUrl: './member-card.component.css'
})
export class MemberCardComponent {
 member = input.required<Member>();
}
