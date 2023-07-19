using FileExplorer.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Repo
{
    public interface IStorageRepository : IRepository<FileModel>
    {
        Task<bool> DeleteFilesOrFoldersAsync(IEnumerable<string> fileIds, string userId);
        IQueryable<FileModel> GetFilesInPath(string pathId, string userId);
        Task<FileModel> GetFullPropertiesAsync(string fileId);
        Task<List<FileModel>> ListWithPaginationAsync(string parentId, string userId, int page, int numberOfRecords);
        Task<bool> RenameAsync(FileModel fileModel, string newName);
        Task<bool> CreateStorageItemsAsync(List<FileModel> files);
        Task<string> GetParentPathFromIdAsync(string parentId);
        Task<bool> UpdateNewPathAsync(List<ItemDataForMoveDTO> data);
    }
}
