import { Component, OnDestroy } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { catchError, combineLatest, exhaustMap, filter, first, map, merge, Observable, of, pipe, shareReplay, startWith, Subject, switchMap, tap } from 'rxjs';
import { CreatePlayerCommand, PlayerDto, PlayersClient, UpdatePlayerCommand } from 'src/app/core/clients/web-api-clients';
import { CreateEditPlayerDialogComponent } from './create-edit-player-dialog/create-edit-player-dialog.component';
import { AuthService } from 'src/app/core/services/auth.service';
import { DialogService } from 'src/app/core/services/dialog.service';
import { GameService } from '../../services/game.service';
import { LobbyHubService } from '../../services/lobby-hub.service';
import { PlayersService } from '../../services/players.service';

const CREATE_PLAYER_DIALOG_CONFIG: MatDialogConfig = {
  disableClose: true
}
@Component({
  selector: 'app-players',
  templateUrl: './players.component.html',
  styleUrls: ['./players.component.scss']
})
export class PlayersComponent implements OnDestroy {

  public players$ = this.playersService.players$;
  public thisPlayer$ = this.playersService.thisPlayer$
  public thisPlayerAdmin$ = this.thisPlayer$.pipe(map(p => p?.gameAdmin), shareReplay());
  public game$ = this.gameService.game$;

  public playersNumber$ = combineLatest([this.game$, this.players$]).pipe(
    map(([game, players]) => `${players.length}/${game?.maxPlayers}`))

  constructor(
    private lobbyHub: LobbyHubService,
    private gameService: GameService,
    public playersService: PlayersService,
    public dialog: DialogService,
    public auth: AuthService) {
    this.init();
  }

  private init() {
    this.thisPlayer$.pipe(
      first(),
      filter(player => !player),
      switchMap(_ => this.createPlayer())
    ).subscribe();

    merge(this.lobbyHub.playersUpdate$, this.lobbyHub.playerLeft$, this.lobbyHub.playerKick$, this.lobbyHub.playerJoined$)
      .subscribe(_ => this.playersService.reload());
  }

  private createPlayer() {
    const data = { player: { name: this.auth.userProfile?.username }, edit: false };
    return this.dialog.open(CreateEditPlayerDialogComponent, data, CREATE_PLAYER_DIALOG_CONFIG).pipe(
      filter(name => !!name),
      switchMap((player: PlayerDto) => this.playersService.create(player)),
    );
  }

  public kickPlayer(player: PlayerDto) {
    return this.dialog.openConfirmation({
      title: 'Kick Player',
      message: `Do you want to kick player: ${player.name}?`
    }).pipe(
      filter(ok => !!ok),
      switchMap(_ => this.playersService.delete(player.id)),
    ).subscribe();
  }

  public editPlayer(player: PlayerDto) {
    if (!player.thisPlayer) {
      return;
    }
    const data = { player: { ...player }, edit: true }
    this.dialog.open(CreateEditPlayerDialogComponent, data).pipe(
      filter(playerdto => !!playerdto),
      switchMap((playerdto: PlayerDto) => this.updatePlayer(player, playerdto)),
    ).subscribe(dto => {
      Object.assign(player, dto);
    });
  }

  private updatePlayer(player: PlayerDto, change: Partial<PlayerDto>): Observable<PlayerDto> {
    const newPlayer = <PlayerDto>{ ...player, ...change };
    return this.playersService.update(newPlayer).pipe(
      map(_ => newPlayer),
      catchError(_ => of(player)),
      startWith(newPlayer)
    );
  }

  ngOnDestroy(): void {
    this.thisPlayer$.pipe(
      first(),
      filter(player => !!player),
      switchMap(player => this.playersService.delete(player?.id)),
      catchError(_ => of(null))
    ).subscribe();
  }
}
