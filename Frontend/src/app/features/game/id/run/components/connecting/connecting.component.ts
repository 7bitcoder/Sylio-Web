import { Component } from '@angular/core';
import { HubConnectionState } from '@microsoft/signalr';
import { map } from 'rxjs/operators';
import { ConnectionState, GameHubService, State } from '../../services/game-hub.service';

@Component({
  selector: 'app-connecting',
  templateUrl: './connecting.component.html',
  styleUrls: ['./connecting.component.scss']
})
export class ConnectingComponent {

  info$ = this.gameHub.state$.pipe(map(s => this.mapToInfo(s)))

  constructor(private gameHub: GameHubService) { }

  mapToInfo(state: State): string {
    switch (state) {
      case HubConnectionState.Disconnected: return "Disconnected";
      case HubConnectionState.Connected:
      case HubConnectionState.Connecting: return "Connecting";
      case ConnectionState.GameConnected: return "Connected";
      case HubConnectionState.Disconnecting: return "Disconnecting";
      case HubConnectionState.Reconnecting: return "Reconnecting";
    }
  }
}
