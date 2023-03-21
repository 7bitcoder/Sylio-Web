import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GameResolver } from '../../core/resolvers/game.resolver';
import { PlayersResolver } from '../../core/resolvers/players.resolver';
import { DeletedComponent } from './info/deleted/deleted.component';
import { KickedComponent } from './info/kicked/kicked.component';
import { GameListComponent } from './list/game-list.component';
import { NewGameComponent } from './new/new-game.component';
import { GameNotFoundComponent } from './info/not-found/game-not-found.component';
import { GameRunComponent } from './id/run/game-run.component';
import { GameLobbyComponent } from './id/lobby/game-lobby.component';
import { GameStatsComponent } from './id/stats/game-stats.component';
import { RoundsWithStatsResolver } from 'src/app/core/resolvers/rounds-with-stats-resolver';
import { PlayersWithStatsResolver } from 'src/app/core/resolvers/players-with-stats.resolver';

const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'new',
        component: NewGameComponent
      },
      {
        path: 'list',
        component: GameListComponent
      },
      {
        path: 'info',
        children: [
          {
            path: 'not-found',
            component: GameNotFoundComponent
          },
          {
            path: 'kicked',
            component: KickedComponent
          },
          {
            path: 'deleted',
            component: DeletedComponent
          },
        ]
      },
      {
        path: ':id',
        runGuardsAndResolvers: 'always',
        resolve: {
          game: GameResolver,
          players: PlayersResolver
        },
        children: [
          {
            path: 'lobby',
            component: GameLobbyComponent,
          },
          {
            path: 'run',
            component: GameRunComponent,
          },
          {
            path: 'stats',
            component: GameStatsComponent,
            resolve: {
              playerStats: PlayersWithStatsResolver
            }
          },
        ]
      }
    ]
  }
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class GameRoutingModule { }
