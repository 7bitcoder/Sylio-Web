import { Injectable } from '@angular/core';
import { CoodrinatesService } from '../helpers/coodrinates.service';
import { DrawerService } from '../helpers/drawer.service';
import { IElement } from '../interfaces/element';

@Injectable()
export class BorderService implements IElement {

  constructor(private drawer: DrawerService) { }

  update(): void {
  }

  draw(delta: number) {
    this.drawer.drawBorder({ x: 0, y: 0 }, { w: 1920, h: 1080 }, '#ffffff');
  }
}
