import { Component, ElementRef, HostListener, Input, ViewChild, ViewContainerRef } from '@angular/core';
import { FileModel, ListFilesPaginatedResponse } from '../../models/files.dto';
import { ToggleService } from '../../services/toggle.service';
import { FileActionsComponent } from '../file-actions/file-actions.component';

@Component({
  selector: 'app-file-manager',
  templateUrl: './file-manager.component.html',
  styleUrls: ['./file-manager.component.scss'],
})
export class FileManagerComponent {
  @Input() paginatedFiles!: ListFilesPaginatedResponse;
  @ViewChild('fileActions', { read: ViewContainerRef })
  fileActions!: ViewContainerRef;

  constructor(private toggle: ToggleService, private elementRef: ElementRef) {}

  renderActions($event: any, file: FileModel) {
    $event.preventDefault();
    this.toggle.toggleDropdown();
    
    this.fileActions?.clear();

    if (!this.toggle.showDropdown) return;

    const fileActionsRef =
      this.fileActions?.createComponent(FileActionsComponent);
    fileActionsRef!.instance.fileInfo = file;
    console.log('file info: ', file);

    const { clientX, clientY } = $event;
    fileActionsRef.location.nativeElement.style.position = 'absolute';
    fileActionsRef.location.nativeElement.style.left = clientX + 'px';
    fileActionsRef.location.nativeElement.style.top = clientY + 'px';
    console.log(clientX, clientY);
  }

  @HostListener('document:click', ['$event'])
  onClick(event: MouseEvent) {
    const clickedInside = this.elementRef.nativeElement.contains(event.target);
    if (!clickedInside) {
      this.fileActions?.clear();
    }
  }
  
}
