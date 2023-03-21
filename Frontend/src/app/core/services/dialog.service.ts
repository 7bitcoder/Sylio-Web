import { DialogConfig } from '@angular/cdk/dialog';
import { ComponentType } from '@angular/cdk/portal';
import { Injectable } from '@angular/core';
import { MatDialog, MatDialogConfig, MatDialogRef } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { DialogComponent } from '../components/dialog/dialog.component';
import { IDialogData } from '../models/dialog-data';

@Injectable({
  providedIn: 'root'
})
export class DialogService {

  constructor(public dialog: MatDialog) { }

  public open<T>(component: ComponentType<T>, data?: any, config?: MatDialogConfig<any>): Observable<any> {
    config ??= {}
    config.data = data;
    return this.dialog.open(component, config).afterClosed();
  }

  public openInfo<T>(data: IDialogData): Observable<any> {
    data.cancel = undefined;
    data.confirm ??= 'Ok';
    return this.open(DialogComponent, data);
  }

  public openConfirmation<T>(data: IDialogData): Observable<any> {
    data.cancel = 'Cancel';
    data.confirm ??= 'Ok';
    return this.open(DialogComponent, data);
  }
}
