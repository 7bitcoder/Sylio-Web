import { PowerUpGroupType } from "../enums/power-up-group-type";

export interface IGlobalPowerUpGroup {
    type: PowerUpGroupType,
    enabled: boolean,
}