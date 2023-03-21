import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { filter, map, shareReplay, switchMap, tap } from 'rxjs';
import { PlayerDto, GameDto } from 'src/app/core/clients/web-api-clients';
import { DialogService } from 'src/app/core/services/dialog.service';
import { GameService } from '../../services/game.service';
import { PlayersService } from '../../services/players.service';

@Component({
  selector: 'app-admin-controls',
  templateUrl: './admin-controls.component.html',
  styleUrls: ['./admin-controls.component.scss']
})
export class AdminControlsComponent {
  public game$ = this.gameService.game$;

  public enoughPlayers$ = this.playersService.players$.pipe(map(players => players.length > 1), shareReplay());
  public notEnoughPlayers$ = this.enoughPlayers$.pipe(map(r => !r));

  constructor(
    private router: Router,
    private dialog: DialogService,
    private playersService: PlayersService,
    private gameService: GameService) { }

  public start(game: GameDto) {
    this.dialog.openConfirmation({
      title: 'Start Game',
      message: 'Are you sure you want to start game?'
    }).pipe(
      filter(confirm => !!confirm),
      switchMap(_ => this.gameService.start()),
    ).subscribe();
  }


  public delete(game: GameDto) {
    this.dialog.openConfirmation({
      title: 'Delete Game',
      message: 'Are you sure you want to delete game?'
    }).pipe(
      filter(confirm => !!confirm),
      switchMap(_ => this.gameService.delete()),
      tap(_ => this.playersService.dontDeletePLayer = true)
    ).subscribe(_ => this.router.navigate(['home']));
  }
}
