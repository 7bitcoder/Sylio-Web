import { PlayerUpdate } from "../../../../services/game-updates/player-update";
import { SETTINGS } from "../constants/settings";
import { DrawerService } from "../helpers/drawer.service";
import { KeysListenerService, KeyType } from "../helpers/keys-listener.service";
import { PlayerState } from "../models/player-state";
import { Point } from "../models/point";
import { Vector } from "../models/vector";


export class Player {

    private blind = false;
    private undefEffect = false;
    private effectTime = 0

    private tail: PlayerState[] = [];
    private current!: PlayerState

    constructor(
        public id: number,
        private drawer: DrawerService,
    ) {
    }

    update(state: PlayerUpdate | undefined): void {
        if (!state) {
            return;
        }
        this.current = {
            color: state.c,
            radious: state.r,
            visible: state.v,
            position: { x: state.x, y: state.y }
        }
        this.blind = !!state.b;
        this.undefEffect = !!state.a;
        if (!this.tail.length) {
            this.tail.push(this.current);
            return;
        }
        this.tail.push(this.current);
    }

    draw(delta: number): void {
        if (!this.blind) {
            this.drawTail();
        }
        this.drawHead(delta);
    }

    reset(): void {
        this.tail = [];
    }

    private drawHead(delta: number) {
        if (!this.current) {
            return;
        }
        this.drawer.beginPath();
        this.drawer.arc(this.current.position, this.current.radious, 0, 2 * Math.PI);
        this.drawer.fillStyle = this.current.color;
        this.drawer.fill();

        this.drawer.beginPath();
        this.drawer.arc(this.current.position, this.current.radious, 0, 2 * Math.PI);
        this.drawer.fillStyle = this.headStyle(delta);
        this.drawer.fill();
    }

    private drawTail() {
        this.drawer.lineCap = "round";
        this.tail.forEach((p, i, arr) => this.drawTailPart(p, arr[i - 1]));
        this.endLine();
    }


    private drawTailPart(position: PlayerState, last: PlayerState | undefined) {
        if (last?.visible != position.visible) {
            return position.visible ? this.beginNewLine(position) : this.endLine();
        }
        if (!position.visible) {
            return;
        }
        if (last?.color !== position.color || last.radious !== position.radious) {
            this.endLine();
            this.beginNewLine(position);
        }
        this.drawLinePart(position);
    }

    private beginNewLine(position: PlayerState) {
        this.drawer.beginPath();
        this.drawer.strokeStyle = position.color;
        this.drawer.lineWidth = position.radious * 2;
    }

    private drawLinePart(position: PlayerState) {
        this.drawer.lineTo(position.position);
    }

    private endLine() {
        this.drawer.stroke();
    }

    private headStyle(delta: number) {
        if (this.undefEffect) {
            this.effectTime += delta;
            return this.blinkStyle(this.effectTime);
        } else {
            this.effectTime = 0;
            return 'rgba(0, 0, 0, 0.3)';
        }
    }

    private blinkStyle(delta: number): string {
        const opacity = 0.5 + Math.sin(delta / 50) * 0.2;
        return `rgba(0, 0, 0, ${opacity})`;
    }
}