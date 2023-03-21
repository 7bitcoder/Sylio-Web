import { AfterViewInit, ChangeDetectionStrategy, Component, ViewChild } from '@angular/core';
import { combineLatest, Observable, of, Subject, withLatestFrom } from 'rxjs';
import { switchMap, debounceTime, filter, catchError, delay, startWith, shareReplay, map, tap } from 'rxjs/operators';
import { GamesClient, GameDto, GamesDto, GameState } from '../../../core/clients/web-api-clients';
import { switchMapWithResult } from 'src/app/core/utils/rxjs-extensions';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTable } from '@angular/material/table';
import { Result } from 'src/app/core/models/result';
import { SKELETON_MOCK } from './skeleton-mock';
import { Router } from '@angular/router';

@Component({
  selector: 'app-game-list',
  templateUrl: './game-list.component.html',
  styleUrls: ['./game-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class GameListComponent implements AfterViewInit {
  GameState = GameState;

  defaultPageSize = 25
  pageSizeOptions = [5, 10, 25, 50, 75];
  displayedColumns = ['created', 'title', 'players', 'roundsNumber', 'inGame'];

  @ViewChild(MatPaginator) paginator: MatPaginator | undefined;

  public search = new Subject<string>();

  public gamesCount$: Observable<number> | undefined;
  public result$: Observable<Result<GamesDto>> | undefined;

  constructor(
    private gamesClient: GamesClient,
    private router: Router) { }

  ngAfterViewInit() {
    const search$ = this.search.asObservable().pipe(
      tap(_ => this.paginator!.firstPage()),
      startWith(''),
    );

    const page$ = this.paginator?.page.pipe(
      startWith(<PageEvent>{ pageIndex: 0, pageSize: this.defaultPageSize })
    )!;

    this.result$ = combineLatest([search$, page$])
      .pipe(
        debounceTime(300),
        switchMapWithResult(([search, page]) => this.getGames(search, page.pageIndex, page.pageSize)),
        map(result => this.prepareResult(result)),
        debounceTime(40),
        shareReplay());

    this.gamesCount$ = this.result$.pipe(map(res => res.value?.count ?? 0));

  }

  public navigate(game: GameDto) {
    this.router.navigate(['game', game.id, 'lobby']);
  }

  private prepareResult(result: Result<GamesDto>): any {
    if (result.loading) {
      result.value = new GamesDto({ games: SKELETON_MOCK, count: SKELETON_MOCK.length })
    }
    return result;
  }

  private getGames(search: string, pageIndex: number, pageSize: number): Observable<GamesDto> {
    return this.gamesClient.getList(search, pageSize, pageIndex);
  }
}
