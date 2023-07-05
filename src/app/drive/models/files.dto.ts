export interface FileModel{
    id: string;
    modifiedDate: string;
    modifiedBy?: string;
    path: string;
    name: string;
    size?: number | null;
    isDirectory: boolean;
    contentType?: string | null;
    extension?: string | null;
    content?: BinaryData | null;
    userId: string;
    parentId?: string;
}

export interface ListFilesPaginatedRequest{
    parentId: string;
    page: number;
    pageSize: number;
}

export interface ListFilesPaginatedResponse{
    files: FileModel[];
    totalCount: number;
    currentPage: number;
}

export interface UploadFileRequest{
    files: File[];
    storageId: string;
}