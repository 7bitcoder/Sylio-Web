
export enum GameUpdateType {
    Player = 1,
    PowerUp = 2,
    Board = 3
}

export interface GameUpdate {
    t: GameUpdateType;
}