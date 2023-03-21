import { PlayerState } from '../../components/game-board/services/models/player-state';
import { GameUpdate } from './game-update';

export interface PlayerUpdate extends GameUpdate {
    id: number;
    v: boolean;
    b: boolean;
    x: number;
    y: number;
    r: number;
    c: string;
    s: number;
    a: boolean;
}
