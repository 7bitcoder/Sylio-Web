import { Injectable, OnDestroy } from '@angular/core';
import { GameHubService } from '../../../../services/game-hub.service';
import { StageType } from '../../../../services/stage-type';
import { KeyControlType } from '../enums/key-type';


export enum KeyType {
  Left = 65,
  Right = 68,
  Space = 32,
  None
}

@Injectable()
export class KeysListenerService implements OnDestroy {
  stage: StageType = StageType.None;

  constructor(private gameHub: GameHubService) {
  }

  init() {
    this.registerKeyListeners();
    this.gameHub.stage$.subscribe(stage => this.stage = stage);
  }

  ngOnDestroy(): void {
    document.onkeydown = null;
    document.onkeyup = null;
  }

  private registerKeyListeners() {
    document.onkeydown = (ev) => this.onKeyDown(ev.keyCode);
    document.onkeyup = (ev) => this.onKeyUp(ev.keyCode);
  }

  private onKeyDown(keyCode: number) {
    if (this.stage == StageType.Round) {
      this.onKeyDownInGame(keyCode);
    } else if (this.stage == StageType.StartRound) {
      this.onKeyDownWaitForReady(keyCode);
    }
  }

  private onKeyUp(keyCode: number) {
    if (this.stage == StageType.Round) {
      this.onKeyUpInGame(keyCode);
    }
  }

  private onKeyDownInGame(keyCode: number) {
    if (keyCode === KeyType.Left) {
      this.gameHub.keyPressed(KeyControlType.Left).subscribe();
    } else if (keyCode === KeyType.Right) {
      this.gameHub.keyPressed(KeyControlType.Right).subscribe();
    }
  }

  private onKeyDownWaitForReady(keyCode: number) {
    if (keyCode === KeyType.Space) {
      this.gameHub.ready().subscribe();
    }
  }

  private onKeyUpInGame(keyCode: number) {
    if (keyCode === KeyType.Left) {
      this.gameHub.keyReleased(KeyControlType.Left).subscribe();
    } else if (keyCode === KeyType.Right) {
      this.gameHub.keyReleased(KeyControlType.Right).subscribe();
    }
  }
}
