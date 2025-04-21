import { CanDeactivateFn } from '@angular/router';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

export const preventUnsavedChangesGuard: CanDeactivateFn<MemberEditComponent> = (component, currentRoute, currentState, nextState) => {

  if (component.editForm?.dirty) {
    return confirm('You have unsaved changes. Do you really want to leave?');
  }
  
  return true;
};

// Can this be done in a more generic way?
// Yes, we can create a generic guard that checks if the component has an editForm property and if it's dirty.
// This way, we can use this guard for any component that has a form and needs to prevent unsaved changes.
// This is a specific guard for the MemberEditComponent, but we can make it more generic by checking if the component has an editForm property.
// This way, we can use this guard for any component that has a form and needs to prevent unsaved changes.
// The generic guard can be implemented as follows:
// import { CanDeactivateFn } from '@angular/router';
// import { FormGroup } from '@angular/forms';
//
// export interface CanComponentDeactivate {
//   canDeactivate: () => boolean | Observable<boolean> | Promise<boolean>;
// }
//
// export const preventUnsavedChangesGuard: CanDeactivateFn<CanComponentDeactivate> = (component) => {
//   if (component instanceof MemberEditComponent) {
//     return component.canDeactivate ? component.canDeactivate() : true;
//   }
//   if (component.editForm instanceof FormGroup) {
//     return component.editForm.dirty ? confirm('You have unsaved changes. Do you really want to leave?') : true;
//   }
//   return true;
// };
