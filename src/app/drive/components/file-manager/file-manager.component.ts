import { Component, Input } from '@angular/core';
import { ListFilesPaginatedResponse } from '../../models/files.dto';

@Component({
  selector: 'app-file-manager',
  templateUrl: './file-manager.component.html',
  styleUrls: ['./file-manager.component.scss']
})
export class FileManagerComponent {
  @Input() paginatedFiles !: ListFilesPaginatedResponse;


}
