import { PowerUpType } from "../clients/web-api-clients";

export enum PowerUpGroupType {
    PlayerGrow,
    PlayerShrink,
    PlayerSpeedUp,
    PlayerSlowDown,
    PlayerBlind,
    PlayerFreeze,
    PlayerLockLeft,
    PlayerLockRight,
    PlayerSwitchControls,
    PlayerImmortal,
    PlayerLongerGaps,
    PlayerGapsMoreOften,
    BrokenWalls,
    CollapseWalls,
}

export const POWER_UP_GROUP_TYPES = Object.values(PowerUpGroupType)
    .filter((v) => !isNaN(Number(v))) as PowerUpGroupType[];

export const POWER_UP_TYPES = Object.values(PowerUpType)
    .filter((v) => !isNaN(Number(v))) as PowerUpType[];