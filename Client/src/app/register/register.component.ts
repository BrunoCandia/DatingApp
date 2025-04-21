import { JsonPipe } from '@angular/common';
import { Component, OnInit, output } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../services/account.service';
import { TextInputComponent } from "../forms/text-input/text-input.component";

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, JsonPipe, TextInputComponent],
  //imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent implements OnInit {
  // usersFromHomeComponent = input.required<any>();
  cancelRegister = output<boolean>();

  model: any = {};

  registerForm: FormGroup = new FormGroup({});

  constructor(
    private accountService: AccountService,
    private toastrService: ToastrService
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.registerForm = new FormGroup({
      userName: new FormControl('', Validators.required),
      password: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
      confirmPassword: new FormControl('', [Validators.required, this.matchValues('password')]),
    });

    this.registerForm.controls['password'].valueChanges.subscribe(() => {
      this.registerForm.controls['confirmPassword'].updateValueAndValidity();
    });
  }

matchValues(matchTo: string) {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value ? null : { isMatching: true };
    };
  }

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
