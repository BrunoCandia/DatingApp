import { Component, input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl, ReactiveFormsModule } from '@angular/forms';
import { BsDatepickerConfig, BsDatepickerModule } from 'ngx-bootstrap/datepicker';

@Component({
  selector: 'app-date-picker',
  imports: [BsDatepickerModule, ReactiveFormsModule],
  templateUrl: './date-picker.component.html',
  styleUrl: './date-picker.component.css',
})
export class DatePickerComponent implements ControlValueAccessor {
  label = input<string>('');
  maxDate = input<Date>();
  bsConfig?: Partial<BsDatepickerConfig>;

  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
    this.bsConfig = {
      containerClass: 'theme-red',
      dateInputFormat: 'DD MMM YYYY'
    };
  }

  get control(): FormControl {
    return this.ngControl.control as FormControl;
  }

  writeValue(_obj: any): void {
  }

  registerOnChange(_fn: any): void {
  }

  registerOnTouched(_fn: any): void {
  }

}
