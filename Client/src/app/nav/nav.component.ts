import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../services/account.service';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { TitleCasePipe } from '@angular/common';

@Component({
  selector: 'app-nav',
  imports: [FormsModule, BsDropdownModule, RouterLink, RouterLinkActive, TitleCasePipe],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css',
})
export class NavComponent {
  model: any = {};  
  
  constructor(private accountService: AccountService, private router: Router, private toastrService: ToastrService) {}

  get isLoggedIn() {
    return this.accountService.isLoggedIn();
  }

  get currentUserName() {
    return this.accountService.currentUser()?.userName;
  }

  login() {
    this.accountService.login(this.model).subscribe({
      next: (response) => {
        console.log(response);
        this.router.navigateByUrl('/members');        
      },
      error: (error) => {
        console.error(error);
        this.toastrService.error(error.error, 'Login Failed');
      },
      complete: () => {
        console.log('Login request completed');
      },
    });
  }

  logout() {    
    this.accountService.logout();
    this.router.navigateByUrl('/');
    console.log('Logged out successfully');
  }
}
