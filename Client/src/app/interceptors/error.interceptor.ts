import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const toastrService = inject(ToastrService);
  
  return next(req).pipe(
    catchError((error) => {
      if (error) {
        switch (error.status) {
          case 400:
            if (error.error.errors) {
              const modelStateErrors = [];
              for (const key in error.error.errors) {
                if (error.error.errors[key]) {
                  modelStateErrors.push(error.error.errors[key]);
                }
              }
              throw modelStateErrors.flat();
            }
            else {
              toastrService.error(error.error, error.status.toString());
            }

            // if (error.error.errors) {
            //   const modelStateErrors = Object.values(error.error.errors).flat();
            //   toastrService.error(modelStateErrors.join('\n'), 'Validation Error');
            // }
            
            break;
          case 401:
            toastrService.error('Unauthorized', error.status);
            break;
          case 404:
            router.navigateByUrl('/not-found');
            break;
          case 500:
            const navigationExtras: NavigationExtras = { state: { error: error.error }};
            router.navigateByUrl('/server-error', navigationExtras);
            break;
          default:
            toastrService.error('An unexpected error occurred', 'Error');
        }
      }
      throw error;
    })
  )
};
