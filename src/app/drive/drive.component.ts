import { Component, ViewChild } from '@angular/core';
import { FileManagerDirective } from './directives/file-manager.directive';
import { FileManagerComponent } from './components/file-manager/file-manager.component';
import { FileService } from './services/FileService';
import { FileModel, ListFilesPaginatedResponse } from './models/files.dto';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-drive',
  templateUrl: './drive.component.html',
  styleUrls: ['./drive.component.scss'],
})
export class DriveComponent {
  @ViewChild(FileManagerDirective, { static: true })
  appFileManager!: FileManagerDirective;
  paginatedFiles!: ListFilesPaginatedResponse;

  ngOnInit(): void {
    this.fileService.getPaginatedFiles(environment.storageId, 1, 10, environment.token).subscribe({
      next: (res) => {
        this.paginatedFiles = res;
      },
      error: (err) => {
        console.log('error: ', err);
      },
      complete: () => {
        this.loadComponent();
      },
    });
  }

  loadComponent() {
    const viewContainerRef = this.appFileManager.viewContainerRef;
    viewContainerRef.clear();

    const refContainer = viewContainerRef.createComponent(FileManagerComponent);
    refContainer.instance.paginatedFiles = this.paginatedFiles;
  }

  constructor(private fileService: FileService) {
    //this.paginatedFiles = this.fileService.getFiles();
  }
}
