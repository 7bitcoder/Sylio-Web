import { Component } from '@angular/core';
import { Mode } from 'src/app/core/enums/mode';

@Component({
  selector: 'app-new-game',
  templateUrl: './new-game.component.html',
  styleUrls: ['./new-game.component.scss']
})
export class NewGameComponent {
  public Mode = Mode;
}
