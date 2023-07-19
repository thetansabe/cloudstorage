using FileExplorer.DataModel;
using Microsoft.AspNetCore.Http;

namespace FileExplorer.Service
{
    public interface IStorageService: IService<FileModel>
    {
        Task<string> CreateFileAsync(CreateEntryDTO file, UserLoggedInfoDTO user);
        Task<string> RenameAsync(RenameEntryDTO info, UserLoggedInfoDTO user);
        Task<FileModel> GetFullPropertiesAsync(string fileId, UserLoggedInfoDTO user);
        Task<bool> DeleteFilesAsync(IEnumerable<string> fileIds, UserLoggedInfoDTO user);
        Task<string> UploadFilesWithContentAsync(
            List<IFormFile> files,
            string userId,
            string userDisplayName,
            string storageId);
        Task<FileResponseDTO> DownloadFileAsync(string fileId, string userId);
        Task<FilesPagedResponseDTO> ListWithPaginationAsync(string parentId, string userId, int page, int pageSize);
        Task<FileResponseDTO> PreviewFilesAsync(string fileId, string userId);
        Task<bool> MoveStorageItemsAsync(MoveEntryDTO entryData, string userId);
    }
}
