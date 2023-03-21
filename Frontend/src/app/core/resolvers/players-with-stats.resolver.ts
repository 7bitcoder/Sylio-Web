import { Injectable } from '@angular/core';
import {
  Router, Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot
} from '@angular/router';
import { catchError, EMPTY, Observable, of, throwError } from 'rxjs';
import { PlayersClient, PlayerWithStatsDto } from '../clients/web-api-clients';

@Injectable({
  providedIn: 'root'
})
export class PlayersWithStatsResolver implements Resolve<PlayerWithStatsDto[]> {
  constructor(private players: PlayersClient) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<PlayerWithStatsDto[]> {
    return this.getPlayers(route.paramMap.get('id'))
      .pipe(catchError(error => this.onError(error)));
  }

  private getPlayers(gameId: string | null): Observable<PlayerWithStatsDto[]> {
    if (!gameId) {
      return throwError(() => new Error('gameId is invalid'))
    }
    return this.players.getForGameWithStats(gameId);
  }

  private onError(error: Error): Observable<PlayerWithStatsDto[]> {
    return EMPTY;
  }
}
