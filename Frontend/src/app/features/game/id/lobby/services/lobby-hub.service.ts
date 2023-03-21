import { Injectable, OnDestroy } from '@angular/core';
import { HttpTransportType, HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { BehaviorSubject, catchError, combineLatest, distinctUntilChanged, filter, first, from, map, Observable, of, pairwise, startWith, Subject, switchMap, tap, withLatestFrom } from 'rxjs';
import { config } from 'src/app/core/constants/config';
import { GameService } from './game.service';
import { PlayersService } from './players.service';

export interface IMessage {
  user: string,
  message: string
};

@Injectable()
export class LobbyHubService {
  private signalRUrl = config.api.url + '/api/hub/lobby';
  private hubConnection = new HubConnectionBuilder()
    .withUrl(this.signalRUrl)
    .withAutomaticReconnect()
    .build();

  private chatMessages = new Subject<IMessage>();
  private playersUpdate = new Subject<void>();
  private gameUpdate = new Subject<void>();
  private playerKick = new Subject<string>();
  private playerLeft = new Subject<string>();
  private playerJoined = new Subject<string>();
  private gameDeleted = new Subject<void>();
  private gameStarted = new Subject<void>();
  private state = new BehaviorSubject<HubConnectionState>(HubConnectionState.Disconnected);

  chatMessages$ = this.chatMessages.asObservable();
  state$ = this.state.asObservable();
  connected$ = this.state$.pipe(map(s => s == HubConnectionState.Connected));
  playersUpdate$ = this.playersUpdate.asObservable();
  gameUpdate$ = this.gameUpdate.asObservable();
  gameDeleted$ = this.gameDeleted.asObservable();
  playerKick$ = this.playerKick.asObservable();
  playerLeft$ = this.playerLeft.asObservable();
  playerJoined$ = this.playerJoined.asObservable();
  gameStarted$ = this.gameStarted.asObservable();


  private gameId$ = this.gameService.gameId$.pipe(distinctUntilChanged());
  private gameIdChange$ = this.gameId$.pipe(startWith(null), pairwise());
  private playerName$ = this.playerService.thisPlayer$.pipe(filter(p => !!p), map(p => p?.name), first());

  constructor(
    private playerService: PlayersService,
    private gameService: GameService) {
    this.init()
  }

  connect(): Observable<void> {
    return from(this.hubConnection.start()).pipe(
      startWith(void 0),
      tap(_ => this.updateState())
    );
  }

  disconnect(): Observable<void> {
    return combineLatest([this.gameId$, this.playerName$]).pipe(
      switchMap(([gameId, playerName]) => this.leaveLobby(gameId, playerName)),
      catchError(_ => of(null)),
      switchMap(_ => from(this.hubConnection.stop())),
      tap(_ => this.updateState())
    );
  }

  sendMessage(message: string): Observable<void> {
    return combineLatest([this.connected$, this.gameId$, this.playerName$]).pipe(
      filter(([c]) => c),
      first(),
      switchMap(([_, id, playerName]) => from(this.hubConnection.send('sendChatMessage', id, playerName, message)))
    )
  }

  private init() {
    this.listenOnEvents();
    this.joinLobbyEvents();
  }

  private listenOnEvents() {
    this.hubConnection.on('chatMessage', (user: string, message: string) => this.chatMessages.next({ user, message }));
    this.hubConnection.on('gameDeleted', () => this.gameDeleted.next());
    this.hubConnection.on('gameUpdated', () => this.gameUpdate.next());
    this.hubConnection.on('playersUpdated', () => this.playersUpdate.next());
    this.hubConnection.on('playerKicked', (playerId: string) => this.playerKick.next(playerId));
    this.hubConnection.on('playerJoined', (playerId: string) => this.playerJoined.next(playerId));
    this.hubConnection.on('playerLeft', (playerId: string) => this.playerLeft.next(playerId));
    this.hubConnection.on('gameStarted', () => this.gameStarted.next());

    this.hubConnection.onclose(() => this.updateState());
    this.hubConnection.onreconnecting(() => this.updateState());
    this.hubConnection.onreconnected(() => this.updateState());
  }

  private updateState() {
    const state = this.hubConnection.state;
    this.state.next(state);
  }

  private joinLobbyEvents() {
    combineLatest([this.connected$, this.gameIdChange$, this.playerName$]).pipe(
      filter(([c]) => c),
      switchMap(([_, [previousGameId, gameId], name]) => this.joinLobby(previousGameId, gameId, name))
    ).subscribe();
  }

  private joinLobby(previous: string | null, current: string | null, name: string | undefined): Observable<void> {
    const leave$ = this.leaveLobby(previous, name);
    const join$ = current ? from(this.hubConnection.send('joinLobby', current, name)) : of(void 0);
    return leave$.pipe(switchMap(_ => join$));
  }

  private leaveLobby(gameId: string | null, name: string | undefined): Observable<void> {
    return gameId ? from(this.hubConnection.send('leaveLobby', gameId, name)) : of(void 0);
  }
}
