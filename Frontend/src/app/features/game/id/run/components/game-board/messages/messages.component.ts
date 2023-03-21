import { Component } from '@angular/core';
import { startWith, map, merge } from 'rxjs';
import { GameHubService } from '../../../services/game-hub.service';
import { PlayersService } from '../../../services/players.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss']
})
export class MessagesComponent {
  private readyMessage$ = this.gameHub.playersReady$.pipe(
    startWith([]),
    map(readyPLayers => this.mapToMessage(readyPLayers)));

  private countDown$ = this.gameHub.roundStartCountDown$;

  message$ = merge(this.readyMessage$, this.countDown$);

  constructor(
    private gameHub: GameHubService,
    private players: PlayersService) { }


  private mapToMessage(readyPLayers: string[]): string {
    if (readyPLayers.includes(this.players.thisPlayer?.id ?? '')) {
      return 'Waiting for other players to be ready';
    }
    return 'Press space when you are ready'
  }
}
