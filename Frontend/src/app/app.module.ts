import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS, HttpClientModule, provideHttpClient } from '@angular/common/http';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { API_BASE_URL } from './core/clients/web-api-clients';
import { RouterNavigationBarComponent } from './core/components/router-navigation-bar/router-navigation-bar.component';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { KeycloakAngularModule, KeycloakService } from 'keycloak-angular';
import { NavBarComponent } from './core/components/nav-bar/nav-bar.component';

import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatMenuModule } from '@angular/material/menu';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { AuthService } from './core/services/auth.service';
import { ErrorCheckerInterceptor } from './core/interceptors/error-checker.interceptor';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { DialogComponent } from './core/components/dialog/dialog.component';
import { MatDialogModule } from '@angular/material/dialog';
import { config } from 'src/app/core/constants/config';

export function initializeAuth(auth: AuthService) {
  return () => auth.init().subscribe();
}

@NgModule({
  declarations: [
    AppComponent,
    RouterNavigationBarComponent,
    NavBarComponent,
    DialogComponent,
  ],
  imports: [
    CommonModule,
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    MatProgressBarModule,
    KeycloakAngularModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatTooltipModule,
    MatSnackBarModule,
    MatMenuModule,
    MatFormFieldModule,
    MatInputModule,
    MatDialogModule,
  ],
  providers: [
    {
      provide: API_BASE_URL,
      useFactory: () => config.api.url,
    },
    {
      provide: APP_INITIALIZER,
      useFactory: initializeAuth,
      deps: [AuthService, KeycloakService],
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorCheckerInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
