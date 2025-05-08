import { Injectable } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { ConfirmDialogComponent } from '../modals/confirm-dialog/confirm-dialog.component';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConfirmService {

  bsModalRef?: BsModalRef

  constructor(private bsModalService: BsModalService) { }

  confirm(
    title = 'Confirmation',
    message = 'Are you sure you want to do this?',
    btnOkText = 'Ok',
    btnCancelText = 'Cancel') {
      const confirm: ModalOptions = {
        initialState: {
          title,
          message,
          btnOkText,
          btnCancelText
        }
      };

    this.bsModalRef = this.bsModalService.show(ConfirmDialogComponent, confirm);

    return this.bsModalRef.onHidden?.pipe(
      map(() => {
        if (this.bsModalRef?.content) {
          return this.bsModalRef.content.result;
        } else {
          return false;
        }
      })
    )
  }
}
