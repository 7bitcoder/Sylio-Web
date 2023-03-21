import { Injectable } from '@angular/core';
import { HttpTransportType, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { BehaviorSubject, map, distinctUntilChanged, startWith, pairwise, filter, first, Observable, from, tap, combineLatest, switchMap, catchError, of, Subject, delay, share, shareReplay } from 'rxjs';
import { KeyControlType } from '../components/game-board/services/enums/key-type';
import { GameService } from './game.service';
import { PlayersService } from './players.service';
import { StageType } from './stage-type';
import { NotificationType } from './notifications/notification-type';
import { GameNotification } from './notifications/game-notification';
import { GameUpdate } from './game-updates/game-update';
import { GameSetup } from './notifications/game-setup';
import { RoundSetup } from './notifications/round-setup';
import { config } from 'src/app/core/constants/config';

export enum ConnectionState {
  GameConnected = 'GameConnected'
};
export type State = ConnectionState | HubConnectionState;



@Injectable()
export class GameHubService {
  private signalRUrl = config.api.url + '/api/hub/game';
  private hub = new HubConnectionBuilder()
    .withUrl(this.signalRUrl)
    .withAutomaticReconnect()
    .build();

  private gameUpdate = new Subject<GameUpdate[]>();
  private notification = new Subject<GameNotification>();
  private state = new BehaviorSubject<State>(HubConnectionState.Disconnected);

  state$ = this.state.asObservable();
  connected$ = this.state$.pipe(map(s => s == ConnectionState.GameConnected));
  gameUpdate$ = this.gameUpdate.asObservable();

  stage$ = this.filterNotification<StageType>(NotificationType.StageChange, true);
  playersConnected$ = this.filterNotification<string[]>(NotificationType.PlayersConnected, true);
  gameSetup$ = this.filterNotification<GameSetup>(NotificationType.GameSetup, true);
  roundSetup$ = this.filterNotification<RoundSetup>(NotificationType.RoundSetup, true);
  playersReady$ = this.filterNotification<string[]>(NotificationType.PlayersReady);
  roundStartCountDown$ = this.filterNotification<string[]>(NotificationType.RoundStartCountDown);

  private gameId$ = this.gameService.gameId$.pipe(distinctUntilChanged());
  private gameIdChange$ = this.gameId$.pipe(startWith(null), pairwise());
  private playerId$ = this.playerService.thisPlayer$.pipe(filter(p => !!p), map(p => p?.id), first());

  constructor(
    private playerService: PlayersService,
    private gameService: GameService) {
    this.init()
  }

  private filterNotification<T>(type: NotificationType, share = false): Observable<T> {
    let observable = this.notification.pipe(
      filter(n => n.type === type),
      map(n => n.payload as T)
    );
    if (share) {
      observable = observable.pipe(shareReplay())
    }
    return observable;
  }

  private init() {
    this.listenOnEvents();
    this.listenOnConnectToGameEvents();
  }

  connect(): Observable<void> {
    return from(this.hub.start()).pipe(
      startWith(void 0),
      tap(_ => this.updateState())
    );
  }

  disconnect(): Observable<void> {
    return combineLatest([this.gameId$, this.playerId$]).pipe(
      switchMap(([gameId, playerId]) => this.disconectFromGame(gameId, playerId)),
      catchError(_ => of(null)),
      switchMap(_ => from(this.hub.stop())),
      tap(_ => this.updateState())
    );
  }

  private listenOnEvents() {
    this.hub.on('notification', (notification: GameNotification) => this.notification.next(notification));
    this.hub.on('u', (updates: GameUpdate[]) => this.gameUpdate.next(updates));

    this.hub.onclose(() => this.updateState());
    this.hub.onreconnecting(() => this.updateState());
    this.hub.onreconnected(() => this.updateState());

    this.playersConnected$.subscribe(_ => this.updateState(ConnectionState.GameConnected));
    this.gameSetup$.subscribe(setup => this.playerService.updatePlayersWithShordIds(setup.playerIdMap));
  }

  private listenOnConnectToGameEvents() {
    const connected = this.state$.pipe(map(state => state === HubConnectionState.Connected));
    combineLatest([connected, this.gameIdChange$, this.playerId$]).pipe(
      filter(([c]) => c),
      delay(1000),
      switchMap(([_, [previousGameId, gameId], name]) => this.connectToGame(previousGameId, gameId, name))
    ).subscribe();
  }

  private connectToGame(previous: string | null, current: string | null, playerId: string | undefined): Observable<void> {
    const leave$ = this.disconectFromGame(previous, playerId);
    const join$ = current ? from(this.hub.send('connectToGame', current, playerId)) : of(void 0);
    return leave$.pipe(switchMap(_ => join$));
  }

  private disconectFromGame(gameId: string | null, playerId: string | undefined): Observable<void> {
    return gameId ? from(this.hub.send('disconectFromGame', gameId, playerId)) : of(void 0);
  }

  ready(): Observable<void> { return this.send('ready'); }

  keyPressed(key: KeyControlType): Observable<void> { return this.send('k', key); }

  keyReleased(key: KeyControlType): Observable<void> { return this.send('r', key); }

  private send(methodName: string, ...args: any[]): Observable<void> {
    return this.connected$.pipe(
      first(),
      filter(c => c),
      switchMap(_ => from(this.hub.send(methodName, ...args)))
    );
  }

  private updateState(state?: State) {
    const real = this.hub.state;
    this.state.next(state ?? real);
  }
}

