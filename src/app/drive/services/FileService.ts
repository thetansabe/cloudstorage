import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {
  ListFilesPaginatedResponse,
  UploadFileRequest,
} from '../models/files.dto';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class FileService {
  constructor(private readonly _http: HttpClient) {}

  ngOnInit(): void {}

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };

  //fake data
  getFiles(parentId: string = 'a'): ListFilesPaginatedResponse {
    const cloudStorageResponse: ListFilesPaginatedResponse = {
      files: [
        {
          path: 'CloudStorage',
          name: 'a',
          size: 15,
          isDirectory: false,
          contentType: 'text/plain',
          extension: '.txt',
          content: null,
          userId: '73ac5962-a7c4-4237-b3b4-85325c36cf1b',
          parentId: 'b5e08ef8-7c07-424a-ad20-c9bec20f7dc2',
          id: '8763c08d-49de-4c27-b721-f7ba04020afa',
          modifiedDate: '2023-06-19T06:56:10.9145638',
          modifiedBy: 'ntdien',
        },
        {
          path: 'CloudStorage',
          name: 'abc',
          size: null,
          isDirectory: false,
          contentType: 'text/plain',
          extension: 's',
          content: null,
          userId: '73ac5962-a7c4-4237-b3b4-85325c36cf1b',
          parentId: 'b5e08ef8-7c07-424a-ad20-c9bec20f7dc2',
          id: '0207bc28-2253-4807-bfe9-02508a40bc86',
          modifiedDate: '2023-06-21T10:34:46.9142378',
          modifiedBy: 'ntdien2',
        },
        {
          path: 'CloudStorage',
          name: 'folder1',
          size: null,
          isDirectory: true,
          contentType: null,
          extension: null,
          content: null,
          userId: '73ac5962-a7c4-4237-b3b4-85325c36cf1b',
          parentId: 'b5e08ef8-7c07-424a-ad20-c9bec20f7dc2',
          id: 'faab6d41-692e-4fbb-ba91-28a0fa32acb8',
          modifiedDate: '2023-06-21T10:34:46.9101508',
          modifiedBy: 'ntdien',
        },
      ],
      totalCount: 1,
      currentPage: 1,
    };

    const folder1Response: ListFilesPaginatedResponse = {
      files: [
        {
          path: 'CloudStorage\\folder1',
          name: 'file1',
          size: 0,
          isDirectory: false,
          contentType: null,
          extension: '.txt',
          content: null,
          userId: '73ac5962-a7c4-4237-b3b4-85325c36cf1b',
          parentId: 'faab6d41-692e-4fbb-ba91-28a0fa32acb8',
          id: '51a11d3a-7534-42d5-8d40-07d6cfea6105',
          modifiedDate: '2023-06-21T10:34:46.9048697',
          modifiedBy: 'ntdien',
        },
      ],
      totalCount: 1,
      currentPage: 1,
    };

    if (parentId === 'a') {
      return cloudStorageResponse;
    }

    return folder1Response;
  }

  //upload file
  postFile(formData: FormData, authToken: string): Observable<any> {
    const endpoint = environment.apiUrl + environment.upload;
    const headers = new HttpHeaders()
                  .set('Authorization', authToken);

    return this._http.post<string>(endpoint, formData, {
      headers,
      reportProgress: true,
      observe: 'events',
    });
  }

  getPaginatedFiles
    (parentId: string, page: number, pageSize: number, authToken: string): 
    Observable<ListFilesPaginatedResponse> 
  {
    const endpoint = environment.apiUrl + environment.paginatedList;
    const headers = new HttpHeaders()
                  .set('Authorization', authToken);

    return this._http.get<ListFilesPaginatedResponse>(endpoint, {
      headers,
      params: {
        parentId: parentId,
        page: page.toString(),
        pageSize: pageSize.toString(),
      },
    });
  }
}
