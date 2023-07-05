import { Component, ViewChild, Inject, Input } from '@angular/core';
import { FileManagerDirective } from './directives/file-manager.directive';
import { FileManagerComponent } from './components/file-manager/file-manager.component';
import { FileService } from './services/FileService';
import { ListFilesPaginatedResponse } from './models/files.dto';
import { DisplayNotification, NotificationType } from './models/noti.dto';
import { DOCUMENT } from '@angular/common';
import { environment } from 'src/environments/environment';
import { HttpEvent, HttpEventType } from '@angular/common/http';
import { pipe, map } from 'rxjs';
import { createFormData } from './helpers/progress.helper';
import { NotiService } from './services/noti.service';

@Component({
  selector: 'app-drive',
  templateUrl: './drive.component.html',
  styleUrls: ['./drive.component.scss']
})

export class DriveComponent {
  @ViewChild(FileManagerDirective, { static: true }) 
    appFileManager!: FileManagerDirective;

  ngOnInit(): void {
    this.loadComponent();
  }

  paginatedFiles: ListFilesPaginatedResponse;
  
  loadComponent() {
    const viewContainerRef = this.appFileManager.viewContainerRef;
    viewContainerRef.clear();

    const refContainer = viewContainerRef.createComponent(FileManagerComponent);
    refContainer.instance.paginatedFiles = this.paginatedFiles;
  }


  constructor(
    private fileService: FileService,
    @Inject(DOCUMENT) private document: Document,
    private noti: NotiService
  ) { 
    this.paginatedFiles = this.fileService.getFiles();
  }

  // turnOnUploader(){
  //   //turn on uploader
  //   const uploader = this.document.getElementById("fileUpload");
  //   uploader?.click();
  // }

  // percentage: number = 0;

  // uploadFile(event: any){
  //   const formData = createFormData(event.target.files, environment.storageId);
    
  //   const handleUpload = this.fileService.postFile(formData).subscribe({
  //     next: (event: HttpEvent<any>) => {
  //       //console.log('event: ', event);
        
  //       if (event.type === HttpEventType.UploadProgress) {
  //         // Calculate and display the progress percentage
  //         const total = event.total || 0.1;
  //         this.percentage = Math.round((event.loaded / total) * 100);

  //       } else if (event.type === HttpEventType.Response) {
  //         // Complete pipeline
  //         // console.log('Upload complete', event.body);
  //         const noti: DisplayNotification = {
  //           title: 'Upload complete',
  //           message: event.body.detail,
  //           type: NotificationType.SUCCESS,
  //           id: this.noti.notifications.length + 1
  //         }
  //         this.noti.addNoti(noti);
  //         handleUpload.unsubscribe();
  //       }
  //     },
  //     error: (err: any) => {
  //       this.percentage = 0;
  //       // console.log('Error: ', err.error);
  //       const noti: DisplayNotification = {
  //         title: 'Upload failed',
  //         message: err.error.detail,
  //         type: NotificationType.FAILED,
  //         id: this.noti.notifications.length + 1
  //       }
  //       this.noti.addNoti(noti);
  //     },
  //     complete: () => {
  //       this.percentage = 0;
  //       console.log('Complete() called');
  //     }
  //   });
  // }

}
