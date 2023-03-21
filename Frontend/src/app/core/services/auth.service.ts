import { Injectable } from '@angular/core';
import { BehaviorSubject, from, Observable, of } from 'rxjs';
import { catchError, filter, map, switchMap, tap, } from 'rxjs/operators';
import { KeycloakEvent, KeycloakEventType, KeycloakOptions, KeycloakService } from 'keycloak-angular';
import { KeycloakProfile } from 'keycloak-js';
import { config } from 'src/app/core/constants/config';

const KEYCLOAK_OPTIONS: KeycloakOptions = {
  config: {
    url: config.identity.url,
    realm: config.identity.realm,
    clientId: config.identity.clientId,
  },
  loadUserProfileAtStartUp: true,
  initOptions: {
    onLoad: 'check-sso',
    pkceMethod: 'S256',
    // this will solved the error 
    checkLoginIframe: true,

  },
  shouldAddToken: (request) => {
    return request.url.startsWith(config.api.url);
  }
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private initializedSubject = new BehaviorSubject<boolean>(false);
  private userProfileSubject = new BehaviorSubject<KeycloakProfile | undefined>(undefined);

  initialized$ = this.initializedSubject.asObservable();

  userProfile$ = this.userProfileSubject.asObservable();

  isAuthenticated$ = this.userProfile$.pipe(map(profile => !!profile));

  isLoggedIn$ = this.initialized$.pipe(filter(init => init), switchMap(_ => this.isAuthenticated$));

  isLoggedOut$ = this.isLoggedIn$.pipe(map(loggedIn => !loggedIn));

  constructor(private keycloak: KeycloakService) {
    keycloak.keycloakEvents$.subscribe((e) => this.onKeyCloakEvent(e));
  }

  init(): Observable<boolean> {
    return from(this.keycloak.init(KEYCLOAK_OPTIONS)).pipe(
      catchError(err => of(false)),
      tap(_ => this.updateProfile()),
      tap(_ => this.updateInitialized())
    );
  }

  login(redirectUri: string = window.origin): Observable<void> {
    return from(this.keycloak.login({ redirectUri })).pipe(
      catchError(_ => of(void 0)),
      tap(_ => this.updateProfile()),
    );
  }

  register(redirectUri: string = window.origin): Observable<void> {
    return from(this.keycloak.register({ redirectUri })).pipe(
      catchError(_ => of(void 0)),
      tap(_ => this.updateProfile()),
    );
  }

  logout(redirectUri: string = window.origin): Observable<void> {
    return from(this.keycloak.logout(redirectUri)).pipe(
      catchError(_ => of(void 0)),
      tap(_ => this.updateProfile()),
    );
  }

  updateToken(): Observable<boolean> {
    return from(this.keycloak.updateToken(20)).pipe(
      catchError(_ => of(false)),
      tap(_ => this.updateProfile()),
    )
  }

  get userProfile(): KeycloakProfile | undefined { return this.userProfileSubject.value; }

  getToken(): Promise<string> { return this.keycloak.getToken(); }

  private async onKeyCloakEvent(e: KeycloakEvent) {
    if (e.type === KeycloakEventType.OnTokenExpired) {
      this.updateToken();
    }
  }

  private updateProfile() {
    this.userProfileSubject.next(this.keycloak.getKeycloakInstance()?.profile);
  }

  private updateInitialized() {
    this.initializedSubject.next(true);
  }
}
