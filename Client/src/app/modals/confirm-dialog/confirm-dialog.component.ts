import { Component } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-confirm-dialog',
  imports: [],
  templateUrl: './confirm-dialog.component.html',
  styleUrl: './confirm-dialog.component.css'
})
export class ConfirmDialogComponent {
  title = '';
  message = '';
  btnOkText = '';
  btnCancelText = '';
  result = false;
  
  constructor(private bsModalRef: BsModalRef) {}

  confirm() {
    this.result = true;
    this.bsModalRef.hide();
  }

  cancel() {
    this.bsModalRef.hide();
  }
}
