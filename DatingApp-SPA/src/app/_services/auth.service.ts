import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { BehaviorSubject } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;
  currentUser: User;
  photoUrl = new BehaviorSubject<string>('../../assets/user.png');
  type: string;
  currentPhotoUrl = this.photoUrl.asObservable(); // store photo url
constructor(private http: HttpClient) { }

changeMemberPhoto(photoUrl: string) {
  this.photoUrl.next(photoUrl);
}
getMemberType() {
  this.type = this.currentUser.type;
  console.log(this.type);
  return this.type;
}
login(model: any) {
    return this.http.post(this.baseUrl + 'login', model)
    .pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          localStorage.setItem('user', JSON.stringify(user.user)),
          localStorage.setItem('type', JSON.stringify(user.type)),
          this.decodedToken = this.jwtHelper.decodeToken(user.token);
          this.currentUser = user.user;
          this.changeMemberPhoto(this.currentUser.photoUrl);
          this.getMemberType();
        }
      })
    );
  }

  register(userOLO: User) {
    return this.http.post(this.baseUrl + 'register', userOLO);
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

}
