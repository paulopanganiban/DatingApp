import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  photoUrl: string;
  toggle = true;
  status = '@ Faculty';
  // temporary
  type: string;
  constructor(public authService: AuthService, private alertify: AlertifyService,
    private router: Router) { }

  ngOnInit() {
    this.authService.currentPhotoUrl.subscribe(
      photoUrl => this.photoUrl = photoUrl
    );
    this.type = this.authService.getMemberType();
    console.log(this.type);
  }
  login() {
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success('Logged in succesfully'),
      this.type = this.authService.getMemberType();
    }, error => {
      this.alertify.error(error);
    }, () => {
      this.router.navigate(['/members']),
      console.log(this.type);
    });
  }
  onFaculty(job: string) {
      this.toggle = !this.toggle;
      this.status = this.toggle ? '@ Faculty' : 'Not @ Faculty';
      console.log(this.status);
    }
  loggedIn() {
    return this.authService.loggedIn();
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.authService.decodedToken = null;
    this.authService.currentUser = null;
    this.alertify.message('Logged out');
    this.router.navigate(['/home']);

  }
}
