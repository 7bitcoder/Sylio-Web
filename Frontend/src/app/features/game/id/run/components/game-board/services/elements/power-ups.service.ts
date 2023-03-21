import { Injectable } from '@angular/core';
import { PowerUpType } from 'src/app/core/clients/web-api-clients';
import { DrawerService } from '../helpers/drawer.service';
import { IElement } from '../interfaces/element';
import { PowerUp } from '../classes/power-up';
import { GameHubService } from '../../../../services/game-hub.service';
import { PowerUpUpdate } from "../../../../services/game-updates/power-up-update";
import { GameUpdateType } from "../../../../services/game-updates/game-update";
import { PowerUpsService as RealPowerUpService } from 'src/app/core/services/power-ups.service copy';
@Injectable()
export class PowerUpsService implements IElement {

  private cnt = 0;

  private powerUps: PowerUp[] = [];

  constructor(
    private gameHub: GameHubService,
    private drawer: DrawerService,
    private powerUpService: RealPowerUpService,
  ) {
    this.init();
  }

  update(): void {
  }

  reset() {
    this.powerUps = [];
  }

  init() {
    this.gameHub.gameUpdate$.subscribe(update => {
      const powerUpsUpdate = update.filter(u => u.t === GameUpdateType.PowerUp) as PowerUpUpdate[];

      powerUpsUpdate.forEach(powerUp => this.updatePowerUp(powerUp));
    })
  }

  updatePowerUp(update: PowerUpUpdate) {
    if (update.s) {
      const powerup = new PowerUp(update.id, update.u, { x: update.x, y: update.y }, this.drawer, this.powerUpService)
      if (powerup.image) {
        this.powerUps.push(powerup)
      }
    } else {
      this.powerUps = this.powerUps.filter(p => p.id !== update.id);
    }
  }

  draw(delta: number): void {
    this.powerUps.forEach(p => p.draw());
  }
}
