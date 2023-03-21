import { Component, Inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { PlayerDto } from 'src/app/core/clients/web-api-clients';
import { COLORS } from 'src/app/core/enums/colors';

const VALIDATION_VALUES = {
  name: {
    min: 1,
    max: 30
  }
}

@Component({
  selector: 'app-create-edit-player-dialog',
  templateUrl: './create-edit-player-dialog.component.html',
  styleUrls: ['./create-edit-player-dialog.component.scss']
})
export class CreateEditPlayerDialogComponent {
  VALIDATION_VALUES = VALIDATION_VALUES;
  COLORS = COLORS;

  edit = !!this.data.edit;

  color = this.data?.player?.colour ?? this.COLORS[Math.floor(Math.random() * this.COLORS.length)];

  form = this.fb.group({
    name: [
      this.data?.player?.name ?? 'New Player',
      { validators: [Validators.required, Validators.minLength(VALIDATION_VALUES.name.min), Validators.maxLength(VALIDATION_VALUES.name.max)] }
    ],
    colour: [
      this.color,
      { validators: [Validators.required] }
    ]
  });

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<CreateEditPlayerDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { player: PlayerDto, edit: boolean }) {
  }

  public cancel() {
    this.dialogRef.close(undefined);
  }

  public save() {
    const result = this.form.value;
    this.dialogRef.close(result);
  }

  public setColor(event: any) {
    const color = event.color.hex;
    this.color = color;
    this.form.patchValue({ colour: color });
  }
}
