import { Component, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-register',
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  // usersFromHomeComponent = input.required<any>();
  cancelRegister = output<boolean>();

  model: any = {};

  constructor(
    private accountService: AccountService,
    private toastrService: ToastrService
  ) {}

  register() {
    console.log(this.model);
    this.accountService.register(this.model).subscribe({
      next: (response) => {
        console.log(response);
        this.cancel();
      },
      error: (error) => {
        console.log(error);
        this.toastrService.error(error.error, 'Register Failed');
      },
    });
  }

  cancel() {
    console.log('cancelled');
    this.cancelRegister.emit(false);
  }
}
