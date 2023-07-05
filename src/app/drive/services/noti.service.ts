import { Injectable } from '@angular/core';
import { DisplayNotification } from '../models/noti.dto';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NotiService {

  notifications: DisplayNotification[] = [];
  notifications$ = new BehaviorSubject(this.notifications);

  constructor() { }

  addNoti(noti: DisplayNotification) {
    this.notifications.push(noti);
    this.notifications$.next([...this.notifications]);
  }

  removeNoti(id: number) {
    this.notifications = this.notifications.filter(noti => noti.id !== id);
    this.notifications$.next([...this.notifications]);
  }

}
