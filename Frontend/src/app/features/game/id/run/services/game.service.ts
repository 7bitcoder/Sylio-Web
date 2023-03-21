import { Injectable } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject, Observable, filter, first, map, switchMap, tap, of, Subject, debounceTime, distinctUntilChanged } from 'rxjs';
import { CreatePlayerCommand, GameDto, GamesClient, IUpdateGameCommand, PlayerDto, UpdateGameCommand, UpdatePlayerCommand } from 'src/app/core/clients/web-api-clients';

@Injectable()
export class GameService {
  private gameSubject = new BehaviorSubject<GameDto | undefined>(undefined);

  public game$ = this.gameSubject.asObservable();

  public gameId$ = this.game$.pipe(
    filter(game => !!game),
    map(game => game?.id as string),
    distinctUntilChanged()
  ) as Observable<string>;

  constructor(private activatedRoute: ActivatedRoute) {
    this.init();
  }

  private init() {
    this.activatedRoute.data.pipe(
      map(data => data['game'] as GameDto | undefined),
      filter(game => !!game)
    ).subscribe(game => this.gameSubject.next(game))
  }

  get game(): GameDto | undefined { return this.gameSubject.value; }
}
