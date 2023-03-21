import { Component } from '@angular/core';
import { combineLatest, map, of } from 'rxjs';
import { PlayerDto } from 'src/app/core/clients/web-api-clients';
import { GameHubService } from '../../services/game-hub.service';
import { Player, PlayersService } from '../../services/players.service';

interface DisplayPlayer {
  name: string,
  points: number,
  colour: string,
  thisPlayer: boolean,
  connected: boolean,
}

@Component({
  selector: 'app-game-players',
  templateUrl: './game-players.component.html',
  styleUrls: ['./game-players.component.scss']
})
export class GamePlayersComponent {

  players$ = combineLatest([this.playersService.players$, this.gameHub.playersConnected$])
    .pipe(map(([players, connected]) => this.mapPlayers(players, connected)));

  constructor(
    private gameHub: GameHubService,
    private playersService: PlayersService) {
  }

  private mapPlayers(players: Player[], connected: string[]): DisplayPlayer[] {
    return players.map(p => <DisplayPlayer>{
      name: p.name,
      colour: p.colour,
      points: p.score,
      thisPlayer: p.thisPlayer,
      connected: connected.includes(p.id ?? '')
    }).sort((a, b) => b.points - a.points);
  }
}
