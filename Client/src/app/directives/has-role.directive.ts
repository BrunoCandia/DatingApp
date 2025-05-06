import { Directive, inject, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { AccountService } from '../services/account.service';

@Directive({
  selector: '[appHasRole]'  //*appHasRole
})
export class HasRoleDirective implements OnInit {

  @Input() appHasRole: string[] = [];
      
  constructor(private accountService: AccountService, private viewContainerRef: ViewContainerRef, private templateRef: TemplateRef<any>) {
    
  }

  ngOnInit(): void {
    if (this.accountService.roles()?.some(r => this.appHasRole.includes(r))) {
      this.viewContainerRef.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainerRef.clear();
    }
  }

}
