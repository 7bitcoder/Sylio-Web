import { Injectable } from '@angular/core';
import { filter } from 'rxjs';
import { GameHubService } from '../../../../services/game-hub.service';
import { PlayerUpdate } from "../../../../services/game-updates/player-update";
import { GameUpdateType } from "../../../../services/game-updates/game-update";
import { Player } from '../classes/player';
import { DrawerService } from '../helpers/drawer.service';
import { KeysListenerService } from '../helpers/keys-listener.service';
import { IElement } from '../interfaces/element';
import { PlayerState } from '../models/player-state';
import { Point } from '../models/point';
import { PlayersService as PlayersStore } from '../../../../services/players.service'

@Injectable()
export class PlayersService implements IElement {

  private playersMap: { [key: number]: Player } = {};
  private players: Player[] = [];

  constructor(
    private playerService: PlayersStore,
    private gameHub: GameHubService,
    private drawer: DrawerService) {

    this.gameHub.gameUpdate$.subscribe(update => {
      const playerUpdates = update.filter(u => u.t === GameUpdateType.Player) as PlayerUpdate[];
      playerUpdates.forEach(update => this.onUpdate(update))

    })

    this.gameHub.gameSetup$.subscribe(setup => this.setPlayers(setup.playerIdMap.map(m => m.shortId)));
  }

  onUpdate(update: PlayerUpdate) {
    this.playersMap[update.id]?.update(update)
    if (update.s) {
      this.playerService.updateScore(update.id, update.s)
    }
  }

  setPlayers(ids: number[]) {
    ids.forEach(id => this.playersMap[id] = new Player(id, this.drawer));
    this.players = Object.values(this.playersMap);
  }

  update(): void {
  }

  draw(delta: number): void {
    this.players.forEach(p => p.draw(delta));
  }

  reset() {
    this.players.forEach(p => p.reset())
  }
}
