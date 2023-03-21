import { Injectable } from '@angular/core';
import { Initializable } from '../interfaces/Initializable';
import { Point } from '../models/point';
import { Vector } from '../models/vector';
import { CoodrinatesService } from './coodrinates.service';

@Injectable()
export class DrawerService implements Initializable {
  private ctx!: CanvasRenderingContext2D

  constructor(
    private coord: CoodrinatesService) { }

  init(ctx: CanvasRenderingContext2D): void {
    this.ctx = ctx;
  }

  drawBorder(point: Point, size: Vector, color: string) {
    const bw = this.coord.borderWidth;
    this.ctx.fillStyle = color;
    const x = this.coord.trX(point.x);
    const y = this.coord.trY(point.y);
    const w = this.coord.trW(size.w);
    const h = this.coord.trH(size.h);

    this.ctx.fillRect(x - bw, y - bw, w + 2 * bw, h + 2 * bw);
    this.ctx.clearRect(x, y, w, h);
  }

  set lineWidth(value: number) { this.ctx.lineWidth = this.coord.trS(value); }

  set strokeStyle(value: string) { this.ctx.strokeStyle = value; }

  set fillStyle(value: string) { this.ctx.fillStyle = value; }

  set lineCap(value: CanvasLineCap) { this.ctx.lineCap = value; }


  beginPath() { this.ctx.beginPath(); }

  stroke() { this.ctx.stroke(); }

  fill() { this.ctx.fill(); }

  lineTo(point: Point) { this.ctx.lineTo(this.coord.trX(point.x), this.coord.trY(point.y)); }

  drawImage(image: CanvasImageSource, point: Point, size: Vector) {
    this.ctx.drawImage(image, this.coord.trX(point.x), this.coord.trY(point.y), this.coord.trW(size.w), this.coord.trH(size.h))
  }

  arc(point: Point, radius: number, startAngle: number, endAngle: number) {
    this.ctx.arc(this.coord.trX(point.x), this.coord.trY(point.y), this.coord.trS(radius), startAngle, endAngle)
  }

  clearAll() { this.ctx.clearRect(0, 0, this.coord.width, this.coord.height); }
}
