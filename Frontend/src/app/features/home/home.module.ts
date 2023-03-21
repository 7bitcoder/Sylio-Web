import { NgModule } from '@angular/core';
import { HomeRoutingModule } from './home-routing.module';

import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

import { HomeComponent } from './home.component';
import { SharedModule } from '../../shared/shared.module';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [HomeComponent],
  imports: [
    CommonModule,
    SharedModule,
    HomeRoutingModule,
    MatIconModule,
    MatButtonModule
  ]
})
export class HomeModule { }
