import { Component, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { filter, first, map, switchMap } from 'rxjs/operators';
import { Mode } from 'src/app/core/enums/mode';
import { GameService } from './services/game.service';
import { LobbyHubService } from './services/lobby-hub.service';
import { PlayersService } from './services/players.service';

@Component({
  selector: 'app-game-lobby',
  templateUrl: './game-lobby.component.html',
  styleUrls: ['./game-lobby.component.scss'],
  providers: [PlayersService, GameService, LobbyHubService]
})
export class GameLobbyComponent implements OnDestroy {
  Mode = Mode;

  public thisPlayer$ = this.playersService.thisPlayer$;
  public isAdmin$ = this.thisPlayer$.pipe(map(p => !!p?.gameAdmin));

  constructor(
    private router: Router,
    private game: GameService,
    private lobbyHub: LobbyHubService,
    private playersService: PlayersService) {
    this.lobbyHub.connect().subscribe();

    this.lobbyHub.gameStarted$.subscribe(_ => this.onStarted())
  }

  onStarted(): void {
    this.playersService.dontDeletePLayer = true;
    const gameId = this.game.game?.id ?? '';
    this.router.navigate(['game', gameId, 'run']);
  }

  ngOnDestroy(): void {
    this.lobbyHub.disconnect().subscribe();
  }
}
