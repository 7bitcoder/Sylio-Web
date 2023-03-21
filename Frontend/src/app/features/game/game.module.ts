import { APP_INITIALIZER, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GameRoutingModule } from './game-routing.module';
import { NewGameComponent } from './new/new-game.component';
import { GameNotFoundComponent } from './info/not-found/game-not-found.component';
import { GameListComponent } from './list/game-list.component';
import { GameRunComponent } from './id/run/game-run.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatSliderModule } from '@angular/material/slider';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatPaginatorModule } from '@angular/material/paginator';
import { NgxSkeletonLoaderConfig, NgxSkeletonLoaderModule } from 'ngx-skeleton-loader';
import { MatDialogModule } from '@angular/material/dialog';
import { MatListModule } from '@angular/material/list';
import { ColorCircleModule } from 'ngx-color/circle';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';

import { SharedModule } from '../../shared/shared.module';
import { CreateEditViewGameComponent } from './shared/components/create-edit-view-game/create-edit-view-game.component';
import { PowerUpsComponent } from './shared/components/create-edit-view-game/power-ups/power-ups.component';
import { MatTooltipModule } from '@angular/material/tooltip';
import { KickedComponent } from './info/kicked/kicked.component';
import { DeletedComponent } from './info/deleted/deleted.component';
import { AdminControlsComponent } from './id/lobby/components/admin-controls/admin-controls.component';
import { ChatComponent } from './id/lobby/components/chat/chat.component';
import { LeaveGameComponent } from './id/lobby/components/leave-game/leave-game.component';
import { CreateEditPlayerDialogComponent } from './id/lobby/components/players/create-edit-player-dialog/create-edit-player-dialog.component';
import { PlayersComponent } from './id/lobby/components/players/players.component';
import { GameLobbyComponent } from './id/lobby/game-lobby.component';
import { GameBoardComponent } from './id/run/components/game-board/game-board.component';
import { GamePlayersComponent } from './id/run/components/game-players/game-players.component';
import { RoundComponent } from './id/run/components/round/round.component';
import { ConnectingComponent } from './id/run/components/connecting/connecting.component';
import { GameStatsComponent } from './id/stats/game-stats.component';
import { MessagesComponent } from './id/run/components/game-board/messages/messages.component';

const skeletonSettings: Partial<NgxSkeletonLoaderConfig> = {
  animation: 'progress-dark',
  theme: {
    background: 'rgb(80, 80, 80)',
    width: '8rem'
  }
};

@NgModule({
  declarations: [
    NewGameComponent,
    GameNotFoundComponent,
    GameListComponent,
    GameRunComponent,
    PlayersComponent,
    CreateEditPlayerDialogComponent,
    CreateEditViewGameComponent,
    PowerUpsComponent,
    ChatComponent,
    LeaveGameComponent,
    AdminControlsComponent,
    GameLobbyComponent,
    KickedComponent,
    DeletedComponent,
    GamePlayersComponent,
    RoundComponent,
    GameBoardComponent,
    ConnectingComponent,
    GameStatsComponent,
    MessagesComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    MatProgressSpinnerModule,
    GameRoutingModule,
    MatCardModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatSliderModule,
    MatCheckboxModule,
    MatExpansionModule,
    MatPaginatorModule,
    MatDialogModule,
    MatListModule,
    ColorCircleModule,
    MatSlideToggleModule,
    MatTooltipModule,
    NgxSkeletonLoaderModule.forRoot(skeletonSettings),
  ],
})
export class GameModule { }
