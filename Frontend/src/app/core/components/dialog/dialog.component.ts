import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { IDialogData } from '../../models/dialog-data';

@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.scss']
})
export class DialogComponent {
  title = this.data.title;
  lines = this.getMessages();
  confirm = this.data.confirm ?? 'Ok';
  cancel = this.data.cancel;

  constructor(
    private dialogRef: MatDialogRef<DialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: IDialogData) {
  }

  public save(confirm: boolean) {
    this.dialogRef.close(confirm);
  }

  private getMessages(): string[] {
    if (Array.isArray(this.data.message)) {
      return this.data.message;
    }
    return [this.data.message];
  }
}
