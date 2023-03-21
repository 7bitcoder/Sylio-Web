import { Injectable } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject, debounceTime, filter, map, Observable, of, shareReplay, Subject, switchMap, tap } from 'rxjs';
import { CreatePlayerCommand, ICreateGameCommand, ICreatePlayerCommand, IUpdatePlayerCommand, PlayerDto, PlayersClient, UpdatePlayerCommand } from 'src/app/core/clients/web-api-clients';
import { GameService } from './game.service';

@Injectable()
export class PlayersService {
  private reloadSubject = new Subject<void>();
  private players = new BehaviorSubject<PlayerDto[]>([]);

  dontDeletePLayer = false;
  public players$ = this.players.asObservable();

  public thisPlayer$ = this.players$.pipe(
    map(players => players.find(p => p.thisPlayer))
  )

  private gameId$ = this.gameService.gameId$;

  constructor(
    private activatedRoute: ActivatedRoute,
    private gameService: GameService,
    private playersClient: PlayersClient) {
    this.init();
  }

  private init() {
    this.activatedRoute.data.pipe(
      map(data => data['players'] as PlayerDto[] | undefined),
      filter(players => !!players)
    ).subscribe(players => this.players.next(players ?? []))

    this.reloadSubject.pipe(
      debounceTime(100),
      switchMap(_ => this.gameId$),
      switchMap(id => this.playersClient.getForGame(id))
    ).subscribe(players => this.players.next(players));
  }

  public reload(): void {
    this.reloadSubject.next();
  }

  public create(player: ICreatePlayerCommand): Observable<string> {
    return this.gameId$.pipe(
      switchMap(gameId => this.playersClient.create(new CreatePlayerCommand({ ...player, gameId }))),
      tap(_ => this.reload())
    );
  }

  public update(player: IUpdatePlayerCommand): Observable<any> {
    return this.playersClient.update(player.id ?? '', new UpdatePlayerCommand({
      id: player.id ?? '',
      name: player.name ?? '',
      colour: player.colour,
    })).pipe(
      tap(_ => this.reload())
    );
  }

  public delete(id: string | undefined): Observable<any> {
    if (this.dontDeletePLayer) {
      return of(null)
    }
    return this.playersClient.delete(id ?? '').pipe(
      tap(_ => this.reload())
    );
  }
}
