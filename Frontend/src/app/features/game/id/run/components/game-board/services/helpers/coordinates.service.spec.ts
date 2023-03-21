import { TestBed } from '@angular/core/testing';
import { Point } from '../models/point';
import { Vector } from '../models/vector';
import { CoodrinatesService } from './coodrinates.service';

describe('CoodrinatesService', () => {
  let service: CoodrinatesService;
  let canvas: HTMLCanvasElement;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CoodrinatesService]
    });
    service = TestBed.inject(CoodrinatesService);
    canvas = document.createElement('canvas');
    service.init(canvas);
    canvas.width = 1920 / 2;
    canvas.height = 1080 / 2;
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('properely get border coordinates', () => {
    const point: Point = { x: 0, y: 0 };
    const size: Vector = { w: 1920, h: 1080 };
    const bw = service.borderWidth;
    const x = service.trX(point.x);
    const y = service.trY(point.y);
    const w = service.trW(size.w);
    const h = service.trH(size.h);

    expect(x - bw).toBe(0);
    expect(y - bw).toBe(0);
    expect(w + 2 * bw).toBe(canvas.width);
    expect(h + 2 * bw).toBe(canvas.height);
  });

  it('properely draw circle', () => {
    const point: Point = { x: 10, y: 10 };
    const radious = 10;
    const bw = service.borderWidth;
    const x = service.trX(point.x);
    const y = service.trY(point.y);
    const r = service.trS(radious);

    expect(r).toBe(0);
    expect(x - r - bw).toBe(0);
    expect(y - r - bw).toBe(0);
  });
});
