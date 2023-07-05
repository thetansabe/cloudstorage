import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DriveRoutingModule } from './drive-routing.module';
import { DriveComponent } from './drive.component';
import { FileManagerComponent } from './components/file-manager/file-manager.component';
import { FileManagerDirective } from './directives/file-manager.directive';
import { IconPipe } from './pipe/icon.pipe';
import { SizePipe } from './pipe/size.pipe';
import { NotiBellComponent } from './components/noti-bell/noti-bell.component';
import { DropDownComponent } from './components/drop-down/drop-down.component';
import { HeaderComponent } from './components/header/header.component';
import { ActionComponent } from './components/action/action.component';

@NgModule({
  declarations: [
    DriveComponent,
    FileManagerComponent,
    FileManagerDirective,
    IconPipe,
    SizePipe,
    NotiBellComponent,
    DropDownComponent,
    HeaderComponent,
    ActionComponent
  ],
  imports: [
    CommonModule,
    DriveRoutingModule
  ]
})
export class DriveModule { }
