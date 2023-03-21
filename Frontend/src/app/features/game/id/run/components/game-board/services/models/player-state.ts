import { Point } from "./point";

export interface PlayerState {
    position: Point,
    radious: number,
    color: string,
    visible: boolean,
}