import { Component } from '@angular/core';

@Component({
  selector: 'app-noti-bell',
  templateUrl: './noti-bell.component.html',
  styleUrls: ['./noti-bell.component.scss']
})
export class NotiBellComponent {
  showDropdown: boolean = false;

  toggleDropdown() {
    console.log('toggleDropdown', this.showDropdown);
    this.showDropdown = !this.showDropdown;
  }
}
