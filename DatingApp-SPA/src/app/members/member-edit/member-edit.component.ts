import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { NgForm } from '@angular/forms';
import { UserService } from 'src/app/_services/user.service';
import { AuthService } from 'src/app/_services/auth.service';
import { PhotoSchedule } from 'src/app/_models/photo-schedule';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
departmentList = [{value: 'CCIS', display: 'CCIS'},
                    {value: 'CAS', display: 'CAS'},
                    {value: 'MITL', display: 'MITL'},
                    {value: 'ETY', display: 'ETY'},
                    {value: 'CMET', display: 'CMET'}];
user: User;
photoSchedule;
photoUrl: string;
photoUrlSched: string;
@ViewChild('editForm') editForm: NgForm;
  constructor(private route: ActivatedRoute,
    private alertify: AlertifyService, private userService: UserService,
    private authService: AuthService) { }
    // @HostListener syntax for closing browser
    @HostListener('window:beforeunload', ['$event'])
    unloadNotification($event: any) {
      if (this.editForm.dirty) {
        $event.returnValue = true;
      }
    }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });
    this.authService.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
  //  this.authService.currentPhotoUrl.subscribe(photoUrlSched => this.photoUrlSched = photoUrlSched);
   // this.photoSchedule = this.authService.currentUser.photoSchedules;
  }

  updateUser() {
    this.userService.updateUser(this.authService.decodedToken.nameid, this.user).subscribe(next => {
      this.alertify.success('Profile updated!');
      this.editForm.reset(this.user);
    }, error => {
      this.alertify.error(error);
    });
  }

  updateMainPhoto(photoUrl) {
    this.user.photoUrl = photoUrl;
  }
}
