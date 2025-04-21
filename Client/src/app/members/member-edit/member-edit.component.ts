import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { Member } from '../../models/member';
import { AccountService } from '../../services/account.service';
import { MemberService } from '../../services/member.service';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { FormsModule, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-member-edit',
  imports: [TabsModule, FormsModule],
  templateUrl: './member-edit.component.html',
  styleUrl: './member-edit.component.css'
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm?: NgForm;

  @HostListener('window:beforeunload', ['$event']) notify($event: any) {
    if (this.editForm?.dirty) {
      $event.returnValue = true;
    }
  }

  member?: Member;

  constructor(private accountService: AccountService, private memberService: MemberService, private toastrService: ToastrService) { }
  
  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    const user = this.accountService.currentUser();

    if (!user) return;

    this.memberService.getMember(user.userName).subscribe({
      next: (response) => {
              this.member = response;                        
            },
            error: (error) => {
              console.error(error);
            },
    });
  }

  updateMember() {    

    this.memberService.updateMember(this.editForm?.value).subscribe({
      next: (response) => {
        this.toastrService.success('Profile updated successfully');
        this.editForm?.reset(this.member); // Reset the form with the updated member data
      },
      error: (error) => {
        console.error(error);
        this.toastrService.error('Failed to update profile');
      }
    });    
  }
  
}
