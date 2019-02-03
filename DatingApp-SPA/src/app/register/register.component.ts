import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { User } from '../_models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  userOLO: User;
  registerForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>; // Partial makes the type optional

  constructor(private authService: AuthService, private alertify: AlertifyService,
    private fbpau: FormBuilder, private router: Router) { }

  ngOnInit() {
    this.bsConfig = {
      containerClass: 'theme-dark-blue'
    };
    this.createRegisterForm();
  }
  createRegisterForm() {
    this.registerForm = this.fbpau.group({
      type: ['student'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: [null, Validators.required],
      mclnumber: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', Validators.required]
    }, {validator: this.passwordCompare});
  }
  passwordCompare(anyName: FormGroup) {
    return anyName.get('password').value === anyName.get('confirmPassword').value ? null :
     {'mismatchOLO': true};
  }

  register() {
    // this.authService.register(this.model).subscribe(() => {
    //   this.alertify.success('Registration succesful');
    // }, error => {
    //   this.alertify.error(error);
    // });
    if (this.registerForm.valid) {
      this.userOLO = Object.assign({}, this.registerForm.value);
      this.authService.register(this.userOLO).subscribe(() => {
        this.alertify.success('Registration Complete!');
      }, error => {
        this.alertify.error(error);
      },() => {
        this.authService.login(this.userOLO).subscribe(
          () => {
            this.router.navigate(['/members']); // ano gagawin after registration
          });
      });
    }
    console.log(this.registerForm.value);
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
