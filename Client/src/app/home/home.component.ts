import { Component } from '@angular/core';
import { RegisterComponent } from '../register/register.component';

@Component({
  selector: 'app-home',
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent {
  registerMode = false;
  users: any[] = [];

  constructor() {}

  // ngOnInit(): void {
  //   this.getUsers();
  // }

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  cancelRegisterMode($event: boolean) {
    this.registerMode = $event;
  }

  // getUsers() {
  //   this.httpClient.get('https://localhost:7235/api/user').subscribe({
  //     next: (response) => {
  //       this.users = response as any[];
  //       console.log(response);
  //     },
  //     error: (error) => {
  //       console.error(error);
  //     },
  //     complete: () => {
  //       console.log('Request completed');
  //     },
  //   });
  // }
}
