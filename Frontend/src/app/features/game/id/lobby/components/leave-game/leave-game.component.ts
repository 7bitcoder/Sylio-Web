import { Component, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { filter, first, switchMap, withLatestFrom } from 'rxjs';
import { PlayerDto, GameDto } from 'src/app/core/clients/web-api-clients';
import { DialogService } from 'src/app/core/services/dialog.service';
import { LobbyHubService } from '../../services/lobby-hub.service';
import { PlayersService } from '../../services/players.service';

@Component({
  selector: 'app-leave-game',
  templateUrl: './leave-game.component.html',
  styleUrls: ['./leave-game.component.scss']
})
export class LeaveGameComponent {
  public thisPlayer$ = this.playersService.thisPlayer$;

  constructor(
    private lobbyHub: LobbyHubService,
    private router: Router,
    private dialog: DialogService,
    private playersService: PlayersService) {
    this.init();
  }
  public init() {
    this.lobbyHub.playerKick$.pipe(
      withLatestFrom(this.thisPlayer$),
      filter(([playersId, thisPLayer]) => playersId === thisPLayer?.id)
    ).subscribe(_ => this.router.navigate(['game', 'info', 'kicked']))

    this.lobbyHub.gameDeleted$.pipe(
    ).subscribe(_ => this.router.navigate(['game', 'info', 'deleted']))
  }

  public leave(player: PlayerDto) {
    this.dialog.openConfirmation({
      title: 'Leave Game',
      message: [
        'Are you sure you want to leave game?',
        'The game will be deleted if there are no players left'
      ]
    }).pipe(
      filter(confirm => !!confirm),
    ).subscribe(_ => this.router.navigate(['game', 'list']));
  }
}
