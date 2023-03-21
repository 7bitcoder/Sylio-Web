import { PowerUpGroupType } from "../enums/power-up-group-type";

export interface IPowerUpGroup {
    type: PowerUpGroupType

    player: boolean;
    otherPlayers: boolean;
    allPlayers: boolean;
}