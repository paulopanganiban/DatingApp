import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './_guards/auth.guard';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberListResolver } from './_resolvers/member-list.resolver';

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
    //
    { path: 'members/:id', component: MemberDetailComponent, resolve: {
        user: MemberDetailResolver}
    }, // :id to say route parameter
    //
    { path: 'messages', component: MessagesComponent},
    //
    { path: 'lists', component: ListsComponent},
        ]
    },
    // Before global guard
    // { path: 'members', component: MemberListComponent, canActivate: [AuthGuard]},
    // { path: 'messages', component: MessagesComponent},
    // { path: 'lists', component: ListsComponent},
    { path: '**', redirectTo: '', pathMatch: 'full'},
];
