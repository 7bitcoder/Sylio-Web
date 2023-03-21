import { Component } from '@angular/core';
import { NavigationCancel, NavigationEnd, NavigationError, NavigationStart, Router } from '@angular/router';
import { filter, map } from 'rxjs/operators';

@Component({
  selector: 'app-router-navigation-bar',
  templateUrl: './router-navigation-bar.component.html',
  styleUrls: ['./router-navigation-bar.component.scss']
})
export class RouterNavigationBarComponent {

  public loading$ = this.router.events.pipe(
    map(event => this.mapEvent(event)),
    filter(loading => loading !== null)
  );

  constructor(private router: Router) {
  }

  private mapEvent(event: any) {
    switch (true) {
      case event instanceof NavigationStart:
        return true;
      case event instanceof NavigationEnd:
      case event instanceof NavigationCancel:
      case event instanceof NavigationError:
        return false;
      default:
        return null;
    }
  }
}
