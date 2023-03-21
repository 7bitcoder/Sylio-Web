import { Injectable } from '@angular/core';
import {
  Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot
} from '@angular/router';
import { catchError, EMPTY, Observable, throwError } from 'rxjs';
import { PlayerDto, PlayersClient } from '../clients/web-api-clients';

@Injectable({
  providedIn: 'root'
})
export class PlayersResolver implements Resolve<PlayerDto[]> {

  constructor(private playersClient: PlayersClient) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<PlayerDto[]> {
    return this.getPlayers(route.paramMap.get('id'))
      .pipe(catchError(error => this.onError(error)));
  }

  private getPlayers(gameId: string | null): Observable<PlayerDto[]> {
    if (!gameId) {
      return throwError(() => new Error('gameId is invalid'))
    }
    return this.playersClient.getForGame(gameId);
  }

  private onError(error: Error): Observable<PlayerDto[]> {
    return EMPTY;
  }
}
