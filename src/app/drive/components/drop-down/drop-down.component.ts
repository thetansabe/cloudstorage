import { Component, Input } from '@angular/core';
import { DisplayNotification } from '../../models/noti.dto';

@Component({
  selector: 'app-drop-down',
  templateUrl: './drop-down.component.html',
  styleUrls: ['./drop-down.component.scss']
})
export class DropDownComponent {
  @Input() displayNotis: DisplayNotification[] = [];
}
