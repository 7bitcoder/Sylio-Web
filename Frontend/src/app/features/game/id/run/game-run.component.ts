import { Component, OnDestroy } from '@angular/core';
import { Route, Router } from '@angular/router';
import { delay, filter, map } from 'rxjs';
import { GameHubService } from './services/game-hub.service';
import { GameService } from './services/game.service';
import { PlayersService } from './services/players.service';
import { StageType } from './services/stage-type';

@Component({
  selector: 'app-game-run',
  templateUrl: './game-run.component.html',
  styleUrls: ['./game-run.component.scss'],
  providers: [GameHubService, GameService, PlayersService]
})
export class GameRunComponent implements OnDestroy {

  notConnected$ = this.gameHub.connected$.pipe(map(c => !c));


  constructor(
    private gameService: GameService,
    private router: Router,
    private gameHub: GameHubService) {
    this.gameHub.connect().subscribe();


    gameHub.stage$.pipe(
      filter(stage => stage === StageType.EndGame),
      delay(3000)
    ).subscribe(_ => this.router.navigate(['game', this.gameService.game?.id ?? '', 'stats']))
  }

  ngOnDestroy(): void {
    this.gameHub.disconnect().subscribe();
  }

}
