import { ElementRef, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PowerUpType } from 'src/app/core/clients/web-api-clients';
import { SETTINGS } from './constants/settings';
import { BorderService } from './elements/border.service';
import { PlayersService } from './elements/players.service';
import { PowerUpsService } from './elements/power-ups.service';
import { PowerUpsService as Real } from '../../../../../../../../app/core/services/power-ups.service copy';
import { CoodrinatesService } from './helpers/coodrinates.service';
import { DrawerService } from './helpers/drawer.service';
import { IDrawable } from './interfaces/drawable';
import { CancellationToken } from './models/cancellation-token';
import { KeysListenerService } from './helpers/keys-listener.service';

@Injectable()
export class EngineService {

  private FPS = SETTINGS.fps;

  private canvasRef!: ElementRef<HTMLCanvasElement>;
  private canvas!: HTMLCanvasElement;
  private context!: CanvasRenderingContext2D;

  constructor(
    private keysListener: KeysListenerService,
    private coord: CoodrinatesService,
    private drawer: DrawerService,
    private board: BorderService,
    private players: PlayersService,
    private powerUpsService: Real,
    private powerUps: PowerUpsService) { }

  public async init(canvas: ElementRef<HTMLCanvasElement>) {
    this.keysListener.init();
    this.initCanvas(canvas);
    await this.initElements();
  }

  private initCanvas(canvas: ElementRef<HTMLCanvasElement>) {
    this.canvasRef = canvas;
    this.canvas = this.canvasRef.nativeElement;
    const context = this.canvas.getContext('2d');
    if (!context) {
      throw new Error("Unsupported canvas");
    }
    this.context = context;
  }

  private async initElements() {
    this.coord.init(this.canvas);
    this.drawer.init(this.context);
    await this.powerUpsService.loadImages();
  }

  async run(token: CancellationToken) {
    let then = performance.now();
    const interval = 1000 / this.FPS;
    let delta = 0;
    while (!token.isCancelled) {
      let now = await new Promise(requestAnimationFrame);
      if (now - then < interval - delta) {
        continue;
      }
      delta = Math.min(interval, delta + now - then - interval);
      then = now;

      await this.clear()
      await this.update();
      await this.draw(delta);
    }
    await this.clear();
  }

  reset() {
    this.players.reset();
    this.powerUps.reset();
  }

  private async clear() {
    this.drawer.clearAll();
  }

  private async update() {
    this.coord.update();
    this.players.update();
    this.powerUps.update();
  }

  private async draw(delta: number) {
    this.board.draw(delta);
    this.players.draw(delta);
    this.powerUps.draw(delta);
  }
}
