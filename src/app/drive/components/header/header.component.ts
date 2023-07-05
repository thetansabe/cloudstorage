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
    private _viewContainerRef: ViewContainerRef, 
    private _toggle: ToggleService,
    private _noti: NotiService) { }
  
  notiCount : number = 0;
  displayNotis: DisplayNotification[] = [];

  ngOnInit(){
    this._noti.notifications$.subscribe(notis => {
      this.notiCount = notis.length;
      this.displayNotis = notis.reverse();
      console.log('listen notis from header: ', this.displayNotis);
      
    });
  }

  showNoti(){
    if(this._toggle.showDropdown){
      this.dropdown?.clear();
      const dropdownRef = this.dropdown?.createComponent(DropDownComponent);
      dropdownRef!.instance.displayNotis = this.displayNotis;

    }else{
      this.dropdown?.clear();
    }

    this._toggle.toggleDropdown();
  }
}
