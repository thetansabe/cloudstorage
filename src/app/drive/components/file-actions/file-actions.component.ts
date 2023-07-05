import { Component, Input } from '@angular/core';
import { FileModel } from '../../models/files.dto';

@Component({
  selector: 'app-file-actions',
  templateUrl: './file-actions.component.html',
  styleUrls: ['./file-actions.component.scss']
})
export class FileActionsComponent {
  
  @Input() fileInfo !: FileModel;

  
}
