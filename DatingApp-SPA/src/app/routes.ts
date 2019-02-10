import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './_guards/auth.guard';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { ListsResolver } from './_resolvers/lists.resolver';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent},
    // Creation of dummy route
    {
        path: '', // '' to match the url path. path: 'dummy',// localhost:4200/dummymembers
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
    { path: 'members', component: MemberListComponent, resolve: {
        users: MemberListResolver
    }},
    { path: 'members/:id', component: MemberDetailComponent, resolve: {
        user: MemberDetailResolver}
    },
    { path: 'member/edit', component: MemberEditComponent, resolve: {
        user: MemberEditResolver}, canDeactivate: [PreventUnsavedChanges]
    },
    { path: 'messages', component: MessagesComponent},
    { path: 'lists', component: ListsComponent, resolve: {
       users: ListsResolver}},
        ]
    },
    // Before global guard
    // { path: 'members', component: MemberListComponent, canActivate: [AuthGuard]},
    // { path: 'messages', component: MessagesComponent},
    // { path: 'lists', component: ListsComponent},
    { path: '**', redirectTo: '', pathMatch: 'full'},
];
