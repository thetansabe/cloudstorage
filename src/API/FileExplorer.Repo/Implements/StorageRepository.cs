using FileExplorer.DataConnect;
using FileExplorer.DataModel;
using Microsoft.EntityFrameworkCore;

namespace FileExplorer.Repo
{
    public class StorageRepository : Repository<FileModel>, IStorageRepository
    {
        private readonly FileExplorerContext _context;

        public StorageRepository(FileExplorerContext iContext) : base(iContext)
        {
            _context = iContext;
        }

        private DbSet<FileModel> Query => _context.Set<FileModel>();
        public async Task<bool> CreateStorageItemsAsync(List<FileModel> files)
        {
            foreach (var file in files)
            {
                var isExist = Query
                                .Any(f => f.Name == file.Name
                                          && f.Path == file.Path
                                          && f.UserId == file.UserId
                                          && f.IsDirectory == file.IsDirectory
                                          && f.Extension == file.Extension);

                if (isExist) continue;

                Query.Add(file);
            }

            return await _context.SaveChangesAsync() == files.Count;
        }

        public async Task<string> GetParentPathFromIdAsync(string parentId)
        {
            var parent = await Query
                                .FirstOrDefaultAsync(f => f.Id.ToString() == parentId);

            if (parent == null) return string.Empty;

            return parent.Name;
        }

        public async Task<bool> DeleteFilesOrFoldersAsync(
            IEnumerable<string> fileIds,
            string userId
        )
        {
            Query.RemoveRange(
                Query.Where(f =>
                    fileIds.Contains(f.Id.ToString())
                    && f.UserId.ToString() == userId
                )
            );

            return await _context.SaveChangesAsync() == fileIds.Count();
        }

        public async Task<FileModel> GetFullPropertiesAsync(string fileId)
        {
            return await Query
                            .FirstOrDefaultAsync(f => f.Id.ToString() == fileId);
        }

        public async Task<bool> RenameAsync(FileModel file, string newName)
        {

            var newFile = Query
                    .FirstOrDefault(f => f.Id == file.Id);

            var isExist = Query
                    .Any(f => f.Name == newName
                            && f.Path == file.Path
                            && f.UserId == file.UserId
                            && f.IsDirectory == file.IsDirectory
                            && f.Extension == file.Extension);

            if (isExist) return false;

            newFile.Name = newName;
            newFile.ModifiedDate = DateTime.Now;
            newFile.Extension = file.Extension;

            await UpdateAsync(newFile);

            return true;
        }

        public async Task<List<FileModel>> ListWithPaginationAsync(string parentId, string userId, int page, int numberOfRecords)
        {
            var res =    Query
                            .Where(f => f.UserId.ToString() == userId
                                    && f.ParentId.ToString() == parentId)
                            .OrderBy(f => f.Name)
                            .Skip((page - 1) * numberOfRecords)
                            .Take(numberOfRecords);

            await res.ForEachAsync(f => f.Content = null);
            return await res.ToListAsync();
        }

        public IQueryable<FileModel> GetFilesInPath(string pathId, string userId)
        {
            return Query
                    .Where(f => f.ParentId.ToString() == pathId
                                && f.UserId.ToString() == userId);
        }

        /// <summary>
        /// Update new path to move files or folders
        /// </summary>
        public async Task<bool> UpdateNewPathAsync(List<ItemDataForMoveDTO> data)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (ItemDataForMoveDTO itemData in data)
                {
                    foreach (string id in itemData.SubFileIds)
                    {
                        var file = Query
                            .FirstOrDefault(f =>
                                        f.Id.ToString().ToLower() == id.ToLower());

                        if (file == null) continue;

                        var isExist = Query
                                        .Any(f => f.Name == file.Name
                                                    && f.Path == itemData.NewPath
                                                    && f.UserId == file.UserId
                                                    && f.IsDirectory == file.IsDirectory
                                                    && f.Extension == file.Extension);

                        if (isExist) continue; // name already exist

                        file.Path = itemData.NewPath;
                        file.ModifiedDate = DateTime.Now;
                        file.ParentId = Guid.Parse(itemData.NewParentId);

                        Query.Update(file);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }


}
