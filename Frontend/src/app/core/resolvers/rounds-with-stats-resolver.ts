import { Injectable } from '@angular/core';
import {
  Router, Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot
} from '@angular/router';
import { catchError, EMPTY, Observable, of, throwError } from 'rxjs';
import { RoundDto, RoundsClient, RoundWithStatsDto } from '../clients/web-api-clients';

@Injectable({
  providedIn: 'root'
})
export class RoundsWithStatsResolver implements Resolve<RoundWithStatsDto[]> {

  constructor(private rounds: RoundsClient) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<RoundWithStatsDto[]> {
    return this.getRounds(route.paramMap.get('id'))
      .pipe(catchError(error => this.onError(error)));
  }

  private getRounds(gameId: string | null): Observable<RoundWithStatsDto[]> {
    if (!gameId) {
      return throwError(() => new Error('gameId is invalid'))
    }
    return this.rounds.getForGameWithStats(gameId);
  }

  private onError(error: Error): Observable<RoundWithStatsDto[]> {
    return EMPTY;
  }
}
