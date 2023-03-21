import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanActivateChild, CanLoad, Route, Router, RouterStateSnapshot, UrlSegment, UrlTree } from '@angular/router';
import { filter, first, map, Observable, tap } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationGuard implements CanLoad, CanActivate, CanActivateChild {

  constructor(private authService: AuthService) { }

  canLoad(route: Route, segments: UrlSegment[]): Observable<boolean> {
    const fullPath = segments.reduce((path, currentSegment) => `${path}/${currentSegment.path}`, '');
    return this.isAccessAllowed(fullPath);
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.isAccessAllowed(state.url)
  }

  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.isAccessAllowed(state.url)
  }

  isAccessAllowed(redirectPath: string): Observable<boolean> {
    const redirectUri = window.location.origin + redirectPath;
    return this.authService.isLoggedIn$.pipe(
      first(),
      tap(loggedIn => loggedIn || this.authService.login(redirectUri))
    )
  }
}
