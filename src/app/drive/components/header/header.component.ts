import { Component, ViewChild , ViewContainerRef} from '@angular/core';
import { DropDownComponent } from '../drop-down/drop-down.component';
import { ToggleService } from '../../services/toggle.service';
import { NotiService } from '../../services/noti.service';
import { DisplayNotification } from '../../models/noti.dto';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent {
  @ViewChild('dropdown', { read: ViewContainerRef }) dropdown: ViewContainerRef | undefined;;
  
  constructor(
    private toggle: ToggleService,
    private noti: NotiService) { }
  
  notiCount : number = 0;
  displayNotis: DisplayNotification[] = [];

  ngOnInit(){
    this.noti.notifications$.subscribe(notis => {
      this.notiCount = notis.length;
      this.displayNotis = notis.reverse();
    });
  }

  showNoti(){
    if(this.toggle.showDropdown){
      this.dropdown?.clear();
      const dropdownRef = this.dropdown?.createComponent(DropDownComponent);
      dropdownRef!.instance.displayNotis = this.displayNotis;

    }else{
      this.dropdown?.clear();
    }

    this.toggle.toggleDropdown();
  }
}
