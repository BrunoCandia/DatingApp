import { Routes } from '@angular/router';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { authGuard } from './guards/auth.guard';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { preventUnsavedChangesGuard } from './guards/prevent-unsaved-changes.guard';
import { memberDetailResolver } from './resolvers/member-detail.resolver';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { adminGuard } from './guards/admin.guard';


export const routes: Routes = [
  { path: '', component: HomeComponent },
  { 
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    children: [
      { path: 'members', component: MemberListComponent, canActivate: [authGuard] },
      { path: 'members/:userName', component: MemberDetailComponent, resolve: {member: memberDetailResolver} },
      { path: 'member/edit', component: MemberEditComponent, canDeactivate: [preventUnsavedChangesGuard] },
      { path: 'lists', component: ListsComponent },
      { path: 'messages', component: MessagesComponent },
      { path: 'admin', component: AdminPanelComponent, canActivate: [adminGuard] }
    ]
  },
  { path: 'not-found', component: NotFoundComponent },
  { path: 'server-error', component: ServerErrorComponent },
  { path: '**', component: HomeComponent, pathMatch: 'full' }
];

// export const routes: Routes = [
//   { path: '', component: HomeComponent },
//   { path: 'members', component: MemberListComponent, canActivate: [authGuard] },
//   { path: 'members/:id', component: MemberDetailComponent },
//   { path: 'lists', component: ListsComponent },
//   { path: 'messages', component: MessagesComponent },
//   { path: '**', component: HomeComponent, pathMatch: 'full' } // Wildcard route for a 404 page
// ];
