import { PowerUpType } from 'src/app/core/clients/web-api-clients';
import { Point } from '../../components/game-board/services/models/point';
import { GameUpdate } from './game-update';

export interface PowerUpUpdate extends GameUpdate {
    id: number;
    s: boolean;
    x: number;
    y: number;
    u: PowerUpType;
}
