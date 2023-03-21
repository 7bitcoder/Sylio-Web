import { PowerUpType } from "src/app/core/clients/web-api-clients";
import { DrawerService } from "../helpers/drawer.service";
import { SETTINGS } from "../constants/settings";
import { IElement } from "../interfaces/element";
import { Point } from "../models/point";
import { Vector } from "../models/vector";
import { PowerUpsService } from "src/app/core/services/power-ups.service copy";

export class PowerUp implements IElement {

    public image = this.powerUpService.getData(this.type)?.icon!;

    protected size: Vector = { h: SETTINGS.powerUpSize, w: SETTINGS.powerUpSize };
    constructor(
        public id: number,
        protected type: PowerUpType,
        protected position: Point,
        protected drawer: DrawerService,
        private powerUpService: PowerUpsService,
    ) {
        this.position = {
            x: this.position.x - this.size.w / 2,
            y: this.position.y - this.size.h / 2,
        }
    }

    update(): void {
    }

    draw(): void {
        this.drawer.drawImage(this.image, this.position, this.size);
    }
}