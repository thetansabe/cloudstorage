import { Component, ViewChild, Inject, Input } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { environment } from 'src/environments/environment';
import { HttpEvent, HttpEventType } from '@angular/common/http';
import { ListFilesPaginatedResponse } from '../../models/files.dto';
import { FileService } from '../../services/FileService';
import { NotiService } from '../../services/noti.service';
import { createFormData } from '../../helpers/progress.helper';
import { DisplayNotification, NotificationType } from '../../models/noti.dto';

@Component({
  selector: 'app-action',
  templateUrl: './action.component.html',
  styleUrls: ['./action.component.scss']
})
export class ActionComponent {
  paginatedFiles: ListFilesPaginatedResponse;

  constructor(
    private fileService: FileService,
    @Inject(DOCUMENT) private document: Document,
    private noti: NotiService
  ) { 
    this.paginatedFiles = this.fileService.getFiles();
  }

  turnOnUploader(){
    //turn on uploader
    const uploader = this.document.getElementById("fileUpload");
    uploader?.click();
  }

  percentage: number = 0;

  uploadFile(event: any){
    const formData = createFormData(event.target.files, environment.storageId);
    
    const handleUpload = this.fileService
              .postFile(formData, environment.token).subscribe({
      next: (event: HttpEvent<any>) => {
        //console.log('event: ', event);
        
        if (event.type === HttpEventType.UploadProgress) {
          // Calculate and display the progress percentage
          const total = event.total || 0.1;
          this.percentage = Math.round((event.loaded / total) * 100);

        } else if (event.type === HttpEventType.Response) {
          // Complete pipeline
          // console.log('Upload complete', event.body);
          const noti: DisplayNotification = {
            title: 'Upload complete',
            message: event.body.detail,
            type: NotificationType.SUCCESS,
            id: this.noti.notifications.length + 1
          }
          this.noti.addNoti(noti);
          handleUpload.unsubscribe();
        }
      },
      error: (err: any) => {
        this.percentage = 0;
        // console.log('Error: ', err.error);
        const noti: DisplayNotification = {
          title: 'Upload failed',
          message: err.error.detail,
          type: NotificationType.FAILED,
          id: this.noti.notifications.length + 1
        }
        this.noti.addNoti(noti);
      },
      complete: () => {
        this.percentage = 0;
        console.log('Complete() called');
      }
    });
  }
}
