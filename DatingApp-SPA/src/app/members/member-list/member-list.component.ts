import { Component, OnInit } from '@angular/core';
import { User } from '../../_models/user';
import { UserService } from '../../_services/user.service';
import { AlertifyService } from '../../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { filter } from 'rxjs/operators';
import { Pagination, PaginatedResult } from '../../_models/Pagination';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  users: User[];
  user: User = JSON.parse(localStorage.getItem('user'));
  departmentList = [
                    {value: 'CCIS', display: 'CCIS'},
                    {value: 'CAS', display: 'CAS'}];
  userParams: any = {};
  pagination: Pagination;

  constructor(private userService: UserService, private alertify: AlertifyService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data['users'].result;
      this.pagination = data['users'].pagination;
    });
    if (this.user.department === 'CCIS') {
      console.log(this.user.department);
      this.userParams.department = 'CCIS';
       console.log(this.userParams.department);
    }
    if (this.user.department === 'CAS') {
      console.log(this.user.department);
      this.userParams.department = 'CAS';
      console.log(this.userParams.department);
    }
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    console.log(this.pagination.currentPage);
    this.loadUsers();
  }
  resetFilters() {
    this.loadUsers();
    console.log(this.userParams.department);
  }
  // onAssign(assign) {
  //   this.usersForFilter = this.userService.getUsers().subscribe((users: User[]) => {
  //     this.users.filter(
  //       (anyname) => anyname.department === assign
  //     );
  //   });
  //   console.log(this.usersForFilter);
    loadUsers() {
    this.userService.getUsers(this.pagination.currentPage,
      this.pagination.itemsPerPage, this.userParams).subscribe((resultOLO: PaginatedResult<User[]>) => {
      this.users = resultOLO.result;
      this.pagination = resultOLO.pagination;
      console.log('load users');
    }, error => {
      this.alertify.error(error);
    });
  }
  }




