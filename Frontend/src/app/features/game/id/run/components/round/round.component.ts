import { Component } from '@angular/core';
import { combineLatest, map, startWith } from 'rxjs';
import { GameHubService } from '../../services/game-hub.service';
import { GameService } from '../../services/game.service';

@Component({
  selector: 'app-round',
  templateUrl: './round.component.html',
  styleUrls: ['./round.component.scss']
})
export class RoundComponent {

  private maxRounds$ = this.gameService.game$.pipe(map(g => g?.roundsNumber ?? 0));
  private round$ = this.gameHub.roundSetup$.pipe(map(s => s?.roundNumber ?? 0), startWith(1));
  rounds$ = combineLatest([this.round$, this.maxRounds$]).pipe(map(([round, max]) => `${round}/${max}`));

  constructor(private gameService: GameService,
    private gameHub: GameHubService) { }
}
