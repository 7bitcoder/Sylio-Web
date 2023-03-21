import { Injectable } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject, debounceTime, filter, map, Observable, of, shareReplay, Subject, switchMap, tap } from 'rxjs';
import { CreatePlayerCommand, ICreateGameCommand, ICreatePlayerCommand, IUpdatePlayerCommand, PlayerDto, PlayersClient, UpdatePlayerCommand } from 'src/app/core/clients/web-api-clients';
import { GameService } from './game.service';
import { PlayerIdMap } from './notifications/game-setup';

export interface Player extends PlayerDto {
  shotrId?: number;
  score: number;
}
@Injectable()
export class PlayersService {
  private playersSubject = new BehaviorSubject<Player[]>([]);

  public players$ = this.playersSubject.asObservable();
  public thisPlayer$ = this.players$.pipe(map(players => this.findThisPlayer(players)));

  constructor(private activatedRoute: ActivatedRoute) {
    this.init();
  }

  private init() {
    this.activatedRoute.data.pipe(
      map(data => data['players'] as PlayerDto[] | undefined),
      map(players => players?.map(p => <Player>({ ...p, score: 0 }))),
      filter(players => !!players)
    ).subscribe(players => this.playersSubject.next(players ?? []))
  }


  updatePlayersWithShordIds(map: PlayerIdMap[]) {
    const players = this.players;

    players.forEach(p => p.shotrId = map.find(m => m.playerId == p.id)?.shortId);
    this.playersSubject.next(players);
  }

  updateScore(shordId: number, score: number) {
    const player = this.players.find(p => p.shotrId === shordId);
    if (player) {
      player.score = score;
      this.playersSubject.next(this.players);
    }
  }

  get players(): Player[] { return this.playersSubject.value; }

  get thisPlayer(): Player | undefined { return this.findThisPlayer(this.players); }

  private findThisPlayer(players: Player[]) { return players.find(p => p.thisPlayer); }
}
