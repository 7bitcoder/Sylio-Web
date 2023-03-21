import { Component, ElementRef, OnDestroy, ViewChild } from '@angular/core';
import { EngineService } from './services/engine.service';
import { HELPER_SERVICES } from './services/constants/services';
import { GameHubService } from '../../services/game-hub.service';
import { combineLatest, map, merge, Observable, of, startWith } from 'rxjs';
import { StageType } from '../../services/stage-type';
import { PlayersService } from '../../services/players.service';
import { PlayerDto } from 'src/app/core/clients/web-api-clients';
import { CancellationToken } from './services/models/cancellation-token';
import { KeysListenerService } from './services/helpers/keys-listener.service';

@Component({
  selector: 'app-game-board',
  templateUrl: './game-board.component.html',
  styleUrls: ['./game-board.component.scss'],
  providers: HELPER_SERVICES
})
export class GameBoardComponent implements OnDestroy {
  @ViewChild('canvas', { static: true })
  canvas!: ElementRef<HTMLCanvasElement>;

  private token: CancellationToken = { isCancelled: false };

  startRound$ = this.gameHub.stage$.pipe(map(stage => stage === StageType.StartRound));

  constructor(
    private gameHub: GameHubService,
    private engine: EngineService) { }

  ngAfterViewInit(): void {
    this.init();
  }

  private async init() {
    this.startRound$.subscribe(_ => this.engine.reset())

    await this.engine.init(this.canvas);
    await this.engine.run(this.token)
  }

  ngOnDestroy(): void {
    this.token.isCancelled = true;
  }
}
