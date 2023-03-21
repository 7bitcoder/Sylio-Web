import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PowerUpNamePipe } from './pipes/power-up-name.pipe';
import { DateAsAgoPipe } from './pipes/date-as-ago.pipe';
import { TruncatePipe } from './pipes/truncate.pipe';
import { PowerUpIconPipe } from './pipes/power-up-icon.pipe';

const EXPORTS = [
  PowerUpNamePipe,
  PowerUpIconPipe,
  DateAsAgoPipe,
  TruncatePipe,
]
@NgModule({
  declarations: [
    EXPORTS,
  ],
  imports: [
    CommonModule,
  ],
  exports: [
    EXPORTS
  ]
})
export class SharedModule { }
