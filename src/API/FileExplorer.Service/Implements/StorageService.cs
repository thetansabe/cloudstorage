using FileExplorer.DataModel;
using FileExplorer.Repo;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace FileExplorer.Service
{
    public class StorageService : Service<FileModel>, IStorageService
    {
        /// <summary>
        /// Change image quality for upload and preview
        /// Quality range: 0 - 100
        /// </summary>
        private const int IMAGE_QUALITY = 70;

        private readonly IStorageRepository _repository;

        public StorageService(IRepository<FileModel> repository) : base(repository)
        {
            _repository = (IStorageRepository) repository;
        }

        public async Task<string> CreateFileAsync(CreateEntryDTO file, UserLoggedInfoDTO user)
        {
            // file name validation
            var fileName = file.Name.Trim();

            if (!fileName.ValidateFileName().IsNullOrEmpty())
                return fileName.ValidateFileName();

            file.Name = Path.GetFileNameWithoutExtension(fileName);

            // file extension handling
            file.Extension = file.Extension.HandleFileExtension(fileName);

            // create file or folder in db
            var path = await _repository.GetParentPathFromIdAsync(file.ParentId);
            if(path.IsNullOrEmpty()) return "ParentId not found";

            var newFile = file.ToFileModelForCreate(user, path);

            var res = await _repository
                .CreateStorageItemsAsync(new List<FileModel> { newFile });

            if (!res) return "File duplicated";

            return string.Empty;
        }

        public async Task<bool> DeleteFilesAsync(
            IEnumerable<string> fileIds, 
            UserLoggedInfoDTO user)
        {
            return await _repository
                .DeleteFilesOrFoldersAsync(fileIds, user.UserId);
        }

        public async Task<FileResponseDTO> DownloadFileAsync(string fileId, string userId)
        {
            var fileModel = await _repository.GetFullPropertiesAsync(fileId);

            if(fileModel == null || fileModel.UserId.ToString() != userId) 
                return null;

            return fileModel.ToFileResponse();
        }

        public async Task<FileModel> GetFullPropertiesAsync(string fileId, UserLoggedInfoDTO user)
        {
            var res = await _repository
                .GetFullPropertiesAsync(fileId);

            if(res == null || res.UserId.ToString() != user.UserId) return null;
            return res;
        }

        public async Task<FilesPagedResponseDTO> ListWithPaginationAsync(string parentId, string userId, int page, int pageSize)
        {

            List<FileModel> res = await _repository
                .ListWithPaginationAsync(parentId, userId, page, pageSize);

            FilesPagedResponseDTO response = new()
            {
                Files = res,
                TotalCount = (int)Math.Ceiling(res.Count / (double)pageSize),
                CurrentPage = page
            };

            return response;
        }

        public async Task<string> RenameAsync(RenameEntryDTO info, UserLoggedInfoDTO user)
        {
            var file = await _repository
                .GetFullPropertiesAsync(info.FileId);

            // check if file existed
            if(file == null)
                return "FileId not found";

            // check parent id valid
            if(info.ParentId != file.ParentId.ToString())
                return "ParentId not contain this file";

            // update this logic when we have permission system
            if(user.UserId != file.UserId.ToString())
                return "You don't have permission to rename this file.";

            // file name validation
            var fileName = info.NewName.Trim();

            if (!fileName.ValidateFileName().IsNullOrEmpty())
                return fileName.ValidateFileName();

            info.NewName = Path.GetFileNameWithoutExtension(fileName);

            // file extension handling
            info.Extension = info?.Extension?.HandleFileExtension(fileName);

            var fileModel = info.ToFileModelRename(file, user);

            // rename file in db
            var res = await _repository.RenameAsync(fileModel, info.NewName);

            if(!res)
                return "File name existed!";

            return string.Empty;
        }

        public async Task<string> UploadFilesWithContentAsync(
            List<IFormFile> files,
            string userId, 
            string userDisplayName, 
            string storageId)
        {
            var storagePath = await _repository.GetParentPathFromIdAsync(storageId);
            if(storagePath.IsNullOrEmpty()) return "StorageId not found";

            List<FileModel> fileModels = new();

            foreach (var file in files)
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                var binaryInfo = new BinaryFileDTO()
                {
                    MemoryStream = memoryStream,
                    StoragePath = storagePath,
                    UserId = userId,
                    UserDisplayName = userDisplayName,
                    StorageId = storageId
                };

                var fileModel = file.ToFileModel(binaryInfo);

                if (fileModel.isImage())
                {
                    fileModel.Content = fileModel.CompressImage(IMAGE_QUALITY);
                    fileModel.Size = fileModel.Content.Length;
                }

                fileModels.Add(fileModel);
            }

            var res = await _repository.CreateStorageItemsAsync(fileModels);

            if (!res)
                return "Not all files were uploaded! Not find file or File existed";

            return string.Empty;
        }

        public async Task<FileResponseDTO> PreviewFilesAsync(string fileId, string userId)
        {
            var fileModel = await _repository
                .GetByIdAsync(Guid.Parse(fileId));

            if (fileModel == null) return null;

            if(fileModel.isImage())
                fileModel.Content = fileModel.CompressImage(IMAGE_QUALITY);
            
            return fileModel.ToFileResponse();
        }

        /// <summary>
        /// Move files or folders to new path, also subitems of folders
        /// </summary>
        public async Task<bool> MoveStorageItemsAsync(MoveEntryDTO entryData, string userId)
        {
            //check if new path existed
            var newPath = await _repository.GetParentPathFromIdAsync(entryData.NewPathId);
            if (newPath.IsNullOrEmpty()) return false;

            
            List<FileModel> fileModels = new();
            List<ItemDataForMoveDTO> moveData = new();

            
            foreach (string fileId in entryData.FileIds)
            {
                // get level 1 subitems from ids
                var fileFromId = await _repository
                    .GetFullPropertiesAsync(fileId);

                if (fileFromId == null) continue;
                fileModels.Add(fileFromId);

                // add level 2+ subitems to moveData
                if (!fileFromId.IsDirectory) continue;
                var level2Path = Path.Combine(newPath, fileFromId.Name);
                moveData = 
                    GetSubitemsFromPath(userId, fileFromId.Id.ToString(), level2Path, moveData);
            };            

            // add level 1 subitems to moveData
            moveData.Add(new ItemDataForMoveDTO()
            {
                SubFileIds = entryData.FileIds.AsQueryable(),
                NewPath = newPath,
                NewParentId = entryData.NewPathId,
            });

            // move files
            var moving = await _repository
                .UpdateNewPathAsync(moveData);

            return moving;
        }

        public List<ItemDataForMoveDTO>
            GetSubitemsFromPath(string userId, string parentId, string newPath, List<ItemDataForMoveDTO> subItems)
        {
            // level 2 subitems
            var subFiles = 
                _repository.GetFilesInPath(parentId, userId);

            if (!subFiles.Any()) return subItems;

            subItems.Add(new ItemDataForMoveDTO
            {
                SubFileIds = subFiles.Select(x => x.Id.ToString()).AsQueryable(),
                NewPath = newPath,
                NewParentId = parentId,
            });

            //get other subitems inside folder - level 2+
            foreach (FileModel file in subFiles)
            {
                if (!file.IsDirectory) continue;

                //get grandChildItems from subfolder 
                IQueryable<FileModel> grandChildItems = 
                    _repository.GetFilesInPath(file.Id.ToString(), userId);

                //newPath for grandChildItems
                var newGrandChildPath = Path.Combine(new[] { newPath, file.Name });

                subItems.Add(new ItemDataForMoveDTO
                {
                    SubFileIds = grandChildItems.Select(x => x.Id.ToString()).AsQueryable(),
                    NewPath = newGrandChildPath,
                    NewParentId = file.Id.ToString(),
                });

                // recursive call
                return GetSubitemsFromPath(userId, file.Id.ToString(), newGrandChildPath, subItems);
            }

            return subItems;
        }
    }
}
