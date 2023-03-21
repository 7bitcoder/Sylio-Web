import { AfterViewInit, Component, Input, ViewChild } from '@angular/core';
import { FormArray, FormControl, FormGroup } from '@angular/forms';
import { MatSelectionList } from '@angular/material/list';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { IGlobalPowerUpGroup } from 'src/app/core/models/global-power-up-group';
import { IPowerUpGroup } from 'src/app/core/models/power-up-group';
import { PowerUpsService } from 'src/app/core/services/power-ups.service copy';

@Component({
  selector: 'app-power-ups',
  templateUrl: './power-ups.component.html',
  styleUrls: ['./power-ups.component.scss']
})
export class PowerUpsComponent implements AfterViewInit {

  @ViewChild(MatSelectionList) selection!: MatSelectionList;

  @Input() blockedPowerUps!: FormControl;


  public powerUpTypes = this.powerUpService.powerUpTypes();

  allChecked = false;
  intermediante = false;

  constructor(private powerUpService: PowerUpsService) {
  }

  ngAfterViewInit(): void {
    this.selection.options.forEach(option => option.selected = !this.blockedPowerUps.value.includes(option.value));

    this.checkState();
    this.selection.selectionChange.subscribe(_ => this.checkState())
  }

  toggleCheck(checked: boolean) {
    if (checked) {
      this.selection.selectAll();
    } else {
      this.selection.deselectAll();
    }
    this.checkState();
  }

  checkState() {
    const blocked = this.selection.options.filter(op => !op.selected).map(op => op.value);
    this.allChecked = !blocked.length;
    this.intermediante = !this.allChecked && blocked.length < this.powerUpTypes.length;

    this.blockedPowerUps.setValue(blocked);
  }
}
