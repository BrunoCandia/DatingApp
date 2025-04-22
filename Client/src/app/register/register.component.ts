import { JsonPipe } from '@angular/common';
import { Component, OnInit, output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../services/account.service';
import { TextInputComponent } from "../forms/text-input/text-input.component";
import { DatePickerComponent } from "../forms/date-picker/date-picker.component";
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, JsonPipe, TextInputComponent, DatePickerComponent],
  //imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent implements OnInit {
  // usersFromHomeComponent = input.required<any>();
  cancelRegister = output<boolean>();  

  maxDate: Date = new Date();

  validationErrors: string[] | undefined;

  registerForm: FormGroup = new FormGroup({});

  constructor(
    private accountService: AccountService,
    private toastrService: ToastrService,
    private formBuilder: FormBuilder,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
    console.log(this.maxDate);
  }

  initializeForm() {
    // Using FormBuilder to create the form group and form controls
    this.registerForm = this.formBuilder.group({
      userName: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]],
      gender: ['male'],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required]
    });

    // Alternatively, you can use FormControl directly to create the form group and form controls
    // this.registerForm = new FormGroup({
    //   userName: new FormControl('', Validators.required),
    //   password: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
    //   confirmPassword: new FormControl('', [Validators.required, this.matchValues('password')]),
    // });

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
    const dob = this.getDateOnly(this.registerForm.get('dateOfBirth')?.value);
    this.registerForm.patchValue({dateOfBirth: dob});

    this.accountService.register(this.registerForm.value).subscribe({
      next: (response) => {
        console.log(response);
        this.router.navigateByUrl('/members');
        this.toastrService.success('Registration successful!');
      },
      error: (error) => {
        console.log(error);
        this.toastrService.error(error.error, 'Register Failed');
        this.validationErrors = error;
      },
    });
  }

  cancel() {
    console.log('cancelled');
    this.cancelRegister.emit(false);
  }

  private getDateOnly(dob: string | undefined) {
    if (!dob) return;
    return new Date(dob).toISOString().slice(0, 10);
  } 
}
