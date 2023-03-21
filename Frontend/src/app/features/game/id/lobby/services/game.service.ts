import { Injectable } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject, Observable, filter, first, map, switchMap, tap, of, Subject, debounceTime, distinctUntilChanged } from 'rxjs';
import { CreatePlayerCommand, GameDto, GamesClient, IUpdateGameCommand, PlayerDto, UpdateGameCommand, UpdatePlayerCommand } from 'src/app/core/clients/web-api-clients';

@Injectable()
export class GameService {
  private reloadSubject = new Subject<void>();
  private gameSubject = new BehaviorSubject<GameDto | undefined>(undefined);

  public game$ = this.gameSubject.asObservable();

  public gameId$ = this.game$.pipe(
    filter(game => !!game),
    map(game => game?.id as string),
    distinctUntilChanged()
  ) as Observable<string>;

  constructor(
    private activatedRoute: ActivatedRoute,
    private gamesClient: GamesClient) {
    this.init();
  }

  private init() {
    this.activatedRoute.data.pipe(
      map(data => data['game'] as GameDto | undefined),
      filter(game => !!game)
    ).subscribe(game => this.gameSubject.next(game))

    this.reloadSubject.pipe(
      debounceTime(100),
      switchMap(_ => this.gameId$),
      switchMap(id => this.gamesClient.get(id))
    ).subscribe(game => this.gameSubject.next(game));
  }

  public reload(): void {
    this.reloadSubject.next();
  }

  public update(model: IUpdateGameCommand): any {
    return this.gamesClient.update(model.id ?? '', new UpdateGameCommand(model)).pipe(
      tap(_ => this.reload())
    );
  }

  public delete(): Observable<any> {
    return this.game$.pipe(
      first(),
      switchMap(game => this.gamesClient.delete(game?.id ?? ''))
    )
  }

  public start(): Observable<any> {
    return this.game$.pipe(
      first(),
      switchMap(game => this.gamesClient.start(game?.id ?? ''))
    )
  }

  get game(): GameDto | undefined {
    return this.gameSubject.value;
  }
}
