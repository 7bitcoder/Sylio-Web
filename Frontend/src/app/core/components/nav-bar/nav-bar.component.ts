import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent {

  constructor(public auth: AuthService) {
  }

  login() {
    this.auth.login().subscribe();
  }

  logout() {
    this.auth.logout().subscribe();
  }

  register() {
    this.auth.register().subscribe();
  }
}
