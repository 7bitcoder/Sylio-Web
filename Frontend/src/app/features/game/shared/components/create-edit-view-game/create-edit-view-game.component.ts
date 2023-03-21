import { Component, Optional } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { catchError, combineLatest, filter, first, map, Observable, of, startWith, switchMap } from 'rxjs';
import { GamesClient, CreateGameCommand, GameDto, PlayerDto, PowerUpType } from 'src/app/core/clients/web-api-clients';
import { Mode } from 'src/app/core/enums/mode';
import { PowerUpsService } from 'src/app/core/services/power-ups.service copy';
import { GameService } from '../../../id/lobby/services/game.service';
import { LobbyHubService } from '../../../id/lobby/services/lobby-hub.service';
import { PlayersService } from '../../../id/lobby/services/players.service';

const VALIDATION_VALUES = {
  title: {
    max: 70,
    min: 1
  },
  rounds: {
    min: 1,
    max: 20
  },
  maxPlayers: {
    min: 2,
    max: 8
  }
}
@Component({
  selector: 'app-create-edit-view-game',
  templateUrl: './create-edit-view-game.component.html',
  styleUrls: ['./create-edit-view-game.component.scss']
})
export class CreateEditViewGameComponent {
  VALIDATION_VALUES = VALIDATION_VALUES;
  Mode = Mode;

  public blockedPowerUps = this.fb.control(new Array<PowerUpType>());


  public form = this.fb.group({
    title: ['', { validators: [Validators.required, Validators.minLength(VALIDATION_VALUES.title.min), Validators.maxLength(VALIDATION_VALUES.title.max)] }],
    rounds: [20, { validators: [Validators.required, Validators.min(VALIDATION_VALUES.rounds.min), Validators.max(VALIDATION_VALUES.rounds.max)] }],
    maxPlayers: [4, { validators: [Validators.required, Validators.min(VALIDATION_VALUES.maxPlayers.min), Validators.max(VALIDATION_VALUES.maxPlayers.max)] }],
    isPublic: [true],
    blockedPowerUps: this.blockedPowerUps,
  });

  private game$ = this.gameService?.game$ ?? of(null);

  private player$ = this.playersService?.thisPlayer$ ?? of(null);

  public mode$ = combineLatest([this.game$, this.player$]).pipe(
    map(([game, player]) => this.mapMode(game, player)),
    startWith(Mode.View)
  )

  constructor(
    @Optional() private lobbyHub: LobbyHubService,
    @Optional() private gameService: GameService,
    @Optional() private playersService: PlayersService,
    private gamesClient: GamesClient,
    private powerUpsService: PowerUpsService,
    private router: Router,
    private fb: FormBuilder) {

    this.init();
  }

  private mapMode(game: GameDto | undefined, player: PlayerDto | undefined) {
    switch (true) {
      case !!game && player?.gameAdmin: return Mode.Edit;
      case !!game && !player?.gameAdmin: return Mode.View;
      default: return Mode.Create;
    }
  }

  private init() {
    this.game$.pipe(filter(game => !!game))
      .subscribe(game => this.updateForm(game!));

    this.mode$.subscribe(mode => {
      if (mode === Mode.View) {
        this.form.disable();
      } else {
        this.form.enable();
      }
    });

    this.lobbyHub?.gameUpdate$
      .subscribe(_ => this.gameService.reload());
  }

  private updateForm(game: GameDto): void {
    this.form.patchValue({
      title: game.title!,
      rounds: game.roundsNumber!,
      maxPlayers: game.maxPlayers!,
      isPublic: game.isPublic!,
      blockedPowerUps: game.blockedPowerUps
    })
  }

  public submit(mode: Mode) {
    if (mode == Mode.View) {
      return;
    }
    const model = {
      title: this.form.value.title!,
      roundsNumber: this.form.value.rounds!,
      maxPlayers: this.form.value.maxPlayers!,
      isPublic: this.form.value.isPublic!,
      blockedPowerUps: this.form.value.blockedPowerUps!
    };

    if (mode == Mode.Create) {
      this.gamesClient.create(new CreateGameCommand(model))
        .pipe(catchError(error => this.onError(error)))
        .subscribe(gameId => this.router.navigate(['game', gameId, 'lobby']));
    } else {
      this.game$.pipe(
        first(),
        switchMap(game => this.gameService.update({ ...model, id: game?.id ?? '' })),
        catchError(error => this.onError(error))
      ).subscribe();
    }
  }

  private onError(error: any): Observable<string> {
    return of('')
  }
}
