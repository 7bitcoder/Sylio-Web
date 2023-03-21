import { PowerUpType } from "../clients/web-api-clients";
import { PowerUpData } from "../models/power-up-data";


export const POWER_UP_TYPES = Object.values(PowerUpType)
    .filter((v) => !isNaN(Number(v))) as PowerUpType[];

export const PowerUpDataMap: { [key: number]: PowerUpData } = {
    [PowerUpType.PlayerBlind_All]: {
        type: PowerUpType.PlayerBlind_All,
        iconPath: '/assets/power-up-icons/blind.png',
        description: 'All players cannot see their tails'
    },
    // [PowerUpType.CollapseWalls]: {
    //     type: PowerUpType.CollapseWalls,
    //     iconPath: '/assets/power-up-icons/bounds.png',
    //     description: 'Board bounds are collapsing for a while'
    // },
    [PowerUpType.PlayerCrossWalls_Self]: {
        type: PowerUpType.PlayerCrossWalls_Self,
        iconPath: '/assets/power-up-icons/broken_walls_green.png',
        description: 'Player can pass board bounds'
    },
    [PowerUpType.PlayerCrossWalls_All]: {
        type: PowerUpType.PlayerCrossWalls_All,
        iconPath: '/assets/power-up-icons/broken_walls.png',
        description: 'All players can pass board bounds'
    },
    [PowerUpType.CleanBoard]: {
        type: PowerUpType.CleanBoard,
        iconPath: '/assets/power-up-icons/clean_board.png',
        description: 'Players tails are erased'
    },
    [PowerUpType.PlayerFreeze_Others]: {
        type: PowerUpType.PlayerFreeze_Others,
        iconPath: '/assets/power-up-icons/freeze_red.png',
        description: 'Other players are freezed'
    },
    [PowerUpType.PlayerFreeze_All]: {
        type: PowerUpType.PlayerFreeze_All,
        iconPath: '/assets/power-up-icons/freeze.png',
        description: 'All players are freezed'
    },
    [PowerUpType.PlayerGrow_Others]: {
        type: PowerUpType.PlayerGrow_Others,
        iconPath: '/assets/power-up-icons/grow_up_red.png',
        description: 'Other players grow up'
    },
    [PowerUpType.PlayerGrow_All]: {
        type: PowerUpType.PlayerGrow_All,
        iconPath: '/assets/power-up-icons/grow_up.png',
        description: 'All players grow up'
    },
    [PowerUpType.PlayerImmortal_Self]: {
        type: PowerUpType.PlayerImmortal_Self,
        iconPath: '/assets/power-up-icons/immortal_green.png',
        description: 'Player is immortal'
    },
    [PowerUpType.PlayerLongerGaps_Self]: {
        type: PowerUpType.PlayerLongerGaps_Self,
        iconPath: '/assets/power-up-icons/longer_gaps_green.png',
        description: 'Player gaps are longer'
    },
    [PowerUpType.PlayerLongerGaps_All]: {
        type: PowerUpType.PlayerLongerGaps_All,
        iconPath: '/assets/power-up-icons/longer_gaps.png',
        description: 'All Players gaps are longer'
    },
    [PowerUpType.PlayerGapsMoreOften_Self]: {
        type: PowerUpType.PlayerGapsMoreOften_Self,
        iconPath: '/assets/power-up-icons/more_often_holes_green.png',
        description: 'Player gaps are more often'
    },
    [PowerUpType.PlayerGapsMoreOften_All]: {
        type: PowerUpType.PlayerGapsMoreOften_All,
        iconPath: '/assets/power-up-icons/more_often_holes.png',
        description: 'All Players gaps are more often'
    },
    [PowerUpType.PlayerLockLeft_Others]: {
        type: PowerUpType.PlayerLockLeft_Others,
        iconPath: '/assets/power-up-icons/only_right_red.png',
        description: 'Lock other players left turns'
    },
    [PowerUpType.PlayerLockRight_Others]: {
        type: PowerUpType.PlayerLockRight_Others,
        iconPath: '/assets/power-up-icons/only_left_red.png',
        description: 'Lock other players right turns'
    },
    [PowerUpType.PlayerShrink_Self]: {
        type: PowerUpType.PlayerShrink_Self,
        iconPath: '/assets/power-up-icons/shrink_green.png',
        description: 'Player shrinks down'
    },
    [PowerUpType.PlayerShrink_All]: {
        type: PowerUpType.PlayerShrink_All,
        iconPath: '/assets/power-up-icons/shrink.png',
        description: 'All players shrinks down'
    },
    [PowerUpType.PlayerSlowDown_Self]: {
        type: PowerUpType.PlayerSlowDown_Self,
        iconPath: '/assets/power-up-icons/slow_down_green.png',
        description: 'Player slows down'
    },
    [PowerUpType.PlayerSlowDown_Others]: {
        type: PowerUpType.PlayerSlowDown_Others,
        iconPath: '/assets/power-up-icons/slow_down_red.png',
        description: 'Other players slows down'
    },
    [PowerUpType.PlayerSlowDown_All]: {
        type: PowerUpType.PlayerSlowDown_All,
        iconPath: '/assets/power-up-icons/slow_down.png',
        description: 'All players slows down'
    },
    [PowerUpType.PlayerSpeedUp_Self]: {
        type: PowerUpType.PlayerSpeedUp_Self,
        iconPath: '/assets/power-up-icons/speed_up_green.png',
        description: 'Player speeds up'
    },
    [PowerUpType.PlayerSpeedUp_Others]: {
        type: PowerUpType.PlayerSpeedUp_Others,
        iconPath: '/assets/power-up-icons/speed_up_red.png',
        description: 'Other players speeds up'
    },
    [PowerUpType.PlayerSpeedUp_All]: {
        type: PowerUpType.PlayerSpeedUp_All,
        iconPath: '/assets/power-up-icons/speed_up.png',
        description: 'All players speeds up'
    },
    [PowerUpType.PlayerSpeedUpTurn_Self]: {
        type: PowerUpType.PlayerSpeedUpTurn_Self,
        iconPath: '/assets/power-up-icons/speed_up_green.png',
        description: 'Player turn speeds up'
    },
    [PowerUpType.PlayerSpeedUpTurn_Others]: {
        type: PowerUpType.PlayerSpeedUpTurn_Others,
        iconPath: '/assets/power-up-icons/speed_up_red.png',
        description: 'Other players turn speeds up'
    },
    [PowerUpType.PlayerSpeedUpTurn_All]: {
        type: PowerUpType.PlayerSpeedUpTurn_All,
        iconPath: '/assets/power-up-icons/speed_up.png',
        description: 'All players turn speeds up'
    },
    [PowerUpType.PlayerSlowDownTurn_Self]: {
        type: PowerUpType.PlayerSlowDownTurn_Self,
        iconPath: '/assets/power-up-icons/speed_up_green.png',
        description: 'Player turn slows down'
    },
    [PowerUpType.PlayerSlowDownTurn_Others]: {
        type: PowerUpType.PlayerSlowDownTurn_Others,
        iconPath: '/assets/power-up-icons/speed_up_red.png',
        description: 'Other players turn slows down'
    },
    [PowerUpType.PlayerSlowDownTurn_All]: {
        type: PowerUpType.PlayerSlowDownTurn_All,
        iconPath: '/assets/power-up-icons/speed_up.png',
        description: 'All players turn slows down'
    },
    [PowerUpType.PlayerSwitchControls_Others]: {
        type: PowerUpType.PlayerSwitchControls_Others,
        iconPath: '/assets/power-up-icons/switch_controls_red.png',
        description: 'Other players controls are switched'
    },
    // [PowerUpType.PlayerSwitchHead_All]: {
    //     type: PowerUpType.PlayerSwitchHead_All,
    //     iconPath: '/assets/power-up-icons/switch_head.png',
    //     description: 'Switches all players heads'
    // },
};

