import { trigger, state, style, transition, animate } from '@angular/animations';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { filter, map } from 'rxjs';
import { PlayerRoundStatDto, PlayerWithStatsDto } from 'src/app/core/clients/web-api-clients';

interface Statistic {
  playerId: string,
  playerName: string,
  place: number,
  score: number,
  expanded: boolean,
  roundStats: PlayerRoundStatDto[]
};

@Component({
  selector: 'app-game-stats',
  templateUrl: './game-stats.component.html',
  styleUrls: ['./game-stats.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})
export class GameStatsComponent {
  columnNames = ['place', 'playerName', 'score', 'expanded'];
  displayedColumns: { [key: string]: string } = {
    'place': 'Place',
    'playerName': 'Player Name',
    'score': 'Score',
    'expanded': 'Details'
  };
  innerColumnNames = ['roundNumber', 'place', 'score', 'length', 'liveTime', 'killedBy'];
  innerDisplayColumns: { [key: string]: string } = {
    'roundNumber': 'Round Number',
    'score': 'Score',
    'place': 'Place',
    'length': 'Length',
    'liveTime': 'Lived Time',
    'killedBy': 'Killed By',
  };

  public playerStats$ = this.activatedRoute.data.pipe(
    map(data => data['playerStats'] as PlayerWithStatsDto[]),
    filter(stats => !!stats)
  );

  public basicStats$ = this.playerStats$.pipe(map(stats => this.buildBasicStats(stats)))

  constructor(private activatedRoute: ActivatedRoute) {
  }

  private buildBasicStats(players: PlayerWithStatsDto[]): Statistic[] {
    return players
      .map(p => this.mapToBasicStat(p))
      .sort((a, b) => b.score - a.score)
      .map((p, i) => ({ ...p, place: i + 1 }))
  }

  private mapToBasicStat(player: PlayerWithStatsDto): Statistic {
    return {
      playerName: player.name ?? 'undefined',
      playerId: player.id ?? '',
      place: 0,
      score: player.roundsStats?.map(p => p.score ?? 0).reduce((c, p) => c + p, 0) ?? 0,
      roundStats: player.roundsStats ?? [],
      expanded: false
    };
  }
}
