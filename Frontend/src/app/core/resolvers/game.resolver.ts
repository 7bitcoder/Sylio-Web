import { Injectable } from '@angular/core';
import {
  Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot,
  Router
} from '@angular/router';
import { catchError, EMPTY, Observable, throwError } from 'rxjs';
import { GameDto, GamesClient } from '../clients/web-api-clients';

@Injectable({
  providedIn: 'root'
})
export class GameResolver implements Resolve<GameDto> {

  constructor(
    private router: Router,
    private gamesClient: GamesClient) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<GameDto> {
    return this.getGame(route.paramMap.get('id'))
      .pipe(catchError(error => this.onError(error)));
  }

  private getGame(gameId: string | null): Observable<GameDto> {
    if (!gameId) {
      return throwError(() => new Error('gameId is invalid'))
    }
    return this.gamesClient.get(gameId);
  }

  private onError(error: Error): Observable<GameDto> {
    this.router.navigate(['game', 'info', 'not-found']);
    return EMPTY;
  }
}
