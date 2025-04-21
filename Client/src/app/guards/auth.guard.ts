import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../services/account.service';
import { ToastrService } from 'ngx-toastr';

export const authGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const toastrService = inject(ToastrService);

  if (accountService.isLoggedIn()) {
    return true;
  } else {
    toastrService.error('You are not authorized to view this page', 'Authorization Error');    
    return false;
  }  
};
