import { Injectable } from '@angular/core';
import { SETTINGS } from '../constants/settings';

@Injectable()
export class CoodrinatesService {
  private canvas!: HTMLCanvasElement;
  private parentW = 0;
  private parentH = 0;

  constructor() { }

  init(canvas: HTMLCanvasElement) {
    this.canvas = canvas;
  }

  update() {
    this.checkSize();
  }

  get height() {
    return this.canvas.height;
  }

  get width() {
    return this.canvas.width;
  }

  get borderWidth() {
    return this.width * SETTINGS.borderWidth / SETTINGS.serverMapWidth;
  }

  get mapHeight() {
    return this.height - 2 * this.borderWidth;
  }

  get mapWidth() {
    return this.width - 2 * this.borderWidth;
  }

  trX(x: number): number {
    return this.trW(x) + this.borderWidth;
  }

  trY(y: number): number {
    return this.trH(y) + this.borderWidth;
  }

  trW(w: number): number {
    return this.mapWidth * w / SETTINGS.serverMapWidth;
  }

  trH(h: number): number {
    return this.mapHeight * h / SETTINGS.serverMapHeight;
  }

  trS(s: number): number {
    return this.trW(s);
  }

  private checkSize() {
    if (!this.parentSizeChanged()) {
      return;
    }
    const parentRatio = this.parentW / this.parentH
    const ratio = SETTINGS.serverMapWidth / SETTINGS.serverMapHeight
    if (parentRatio > ratio) {
      this.canvas.height = this.parentH;
      this.canvas.width = ratio * this.parentH;
    } else {
      this.canvas.width = this.parentW;
      this.canvas.height = this.parentW / ratio;
    }
  }

  private parentSizeChanged(): boolean {
    const parent = this.canvas.parentElement!;
    if (this.parentH !== parent.clientHeight || this.parentW !== parent.clientWidth) {
      this.parentH = parent.clientHeight;
      this.parentW = parent.clientWidth;
      return true;
    }
    return false;
  }
}
