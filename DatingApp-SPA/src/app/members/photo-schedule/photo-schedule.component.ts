import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { AuthService } from 'src/app/_services/auth.service';
import { environment } from 'src/environments/environment';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { PhotoSchedule } from 'src/app/_models/photo-schedule';
  @Component({
    selector: 'app-photo-schedule',
    templateUrl: './photo-schedule.component.html',
    styleUrls: ['./photo-schedule.component.css']
  })
  export class PhotoScheduleComponent implements OnInit {
  @Input() photoSchedules: PhotoSchedule[];
  @Output() getMemberPhotoChange = new EventEmitter<string>();
  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  currentMain: PhotoSchedule;

  constructor(private authService: AuthService, private userService: UserService,
    private alertify: AlertifyService) { }
    ngOnInit() {
      this.initializeUploader();
      console.log(this.photoSchedules, 'photoSchedules');
    }
  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/' + this.authService.decodedToken.nameid + '/photoschedule',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });
    this.uploader.onAfterAddingFile = (file) => {file.withCredentials = false; };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res: PhotoSchedule = JSON.parse(response);
        const photoSchedule = {
          id: res.id,
          url: res.url,
          dateAdded: res.dateAdded,
          description: res.description,
          isMain: res.isMainSched
        };
        this.photoSchedules.push(photoSchedule);
      }
    };
  }


  setMainPhoto(photo: PhotoSchedule) {
    this.userService.setMainPhotoSchedule(this.authService.decodedToken.nameid, photo.id).subscribe(
      () => {
        this.currentMain = this.photoSchedules.filter(p => p.isMainSched === true)[0];
        this.currentMain.isMainSched = false;
        photo.isMainSched = true;
        localStorage.setItem('user', JSON.stringify(this.authService.currentUser));
      }, error => {
        console.log(error);
        this.alertify.error(error);
      }
    );
  }

  deletePhoto(id: number) {
    this.alertify.confirm('Are you sure you want to delete?', () => {
      this.userService.deletePhotoSchedule(this.authService.decodedToken.nameid, id).
      subscribe(
      () => {this.photoSchedules.splice(this.photoSchedules.findIndex(p => p.id === id), 1);
        this.alertify.success('Photo has been deleted');
      }, error => {
        this.alertify.error(error);
      });
    });
  }
}
