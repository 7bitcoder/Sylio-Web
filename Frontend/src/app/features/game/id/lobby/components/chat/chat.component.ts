import { AfterViewInit, Component, ElementRef, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { map } from 'rxjs';
import { AuthService } from 'src/app/core/services/auth.service';
import { IMessage, LobbyHubService } from '../../services/lobby-hub.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss'],
})
export class ChatComponent implements AfterViewInit {
  messages: IMessage[] = [];

  @ViewChildren('messageRef') messageRef: QueryList<any> | undefined;
  @ViewChild('messagesRef') messagesRef: ElementRef | undefined;

  disabled$ = this.lobbyHub.connected$.pipe(map(c => !c));

  constructor(
    private lobbyHub: LobbyHubService
  ) {
    this.lobbyHub.chatMessages$
      .subscribe(message => this.messages.push(message))
  }

  sendMessage(element: any) {
    this.lobbyHub.sendMessage(element.value).subscribe();
    element.value = '';
  }


  ngAfterViewInit() {
    this.scrollToBottom();
    this.messageRef?.changes.subscribe(() => this.scrollToBottom());
  }

  private scrollToBottom() {
    try {
      if (this.messagesRef) {
        this.messagesRef.nativeElement.scrollTop = this.messagesRef.nativeElement.scrollHeight;
      }
    } catch (err) { }
  }
}
