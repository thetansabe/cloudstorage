using FileExplorer.DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client.Extensions.Msal;
using Microsoft.IdentityModel.Tokens;
using System.IO;

namespace FileExplorer.Service
{
    public static class EntityExtensions
    {
        public static FileModel ToFileModel(this FileEntryDTO fileEntry)
        {
            return new FileModel
            {
                Id = fileEntry.FileId.IsNullOrEmpty() ? 
                    Guid.NewGuid() : Guid.Parse(fileEntry.FileId),
                Name = fileEntry.Name,
                Path = fileEntry.Path,
                UserId = Guid.Parse(fileEntry.UserId),
                ModifiedBy = fileEntry.UserDisplayName.IsNullOrEmpty() ?
                    fileEntry.UserId : fileEntry.UserDisplayName,
                IsDirectory = fileEntry.IsDirectory,
                Extension = fileEntry.Extension.IsNullOrEmpty() ? 
                    null : fileEntry.Extension,
                ParentId = fileEntry.ParentId.IsNullOrEmpty() ?
                    null : Guid.Parse(fileEntry.ParentId),
                ModifiedDate = DateTime.UtcNow
            };
        }

        public static UserModel ToUserModel(this UserEntryDTO userEntry)
        {
            return new UserModel
            {
                UserName = userEntry.Username,
                HashPassword = userEntry.Password,
                Email = userEntry.Email,
                DisplayName = userEntry.DisplayName.IsNullOrEmpty() ? 
                    userEntry.Username : userEntry.DisplayName,
                ModifiedDate = DateTime.Now,
                ModifiedBy = userEntry.Username,
                Id = Guid.NewGuid()
            };
        }

        public static FileModel ToFileModel(
            this IFormFile file,
            BinaryFileDTO binaryInfo)
        {
            string extension = Path.GetExtension(file.FileName);
            string fileName = Path.GetFileNameWithoutExtension(file.FileName);

            return new FileModel
            {
                Id = Guid.NewGuid(),
                Name = fileName,
                Path = binaryInfo.StoragePath,
                UserId = Guid.Parse(binaryInfo.UserId),
                ModifiedBy = binaryInfo.UserDisplayName,
                IsDirectory = extension.IsNullOrEmpty(),
                Extension = extension,
                ParentId = Guid.Parse(binaryInfo.StorageId),
                ModifiedDate = DateTime.UtcNow,
                Content = binaryInfo.MemoryStream.ToArray(),
                Size = binaryInfo.MemoryStream.Length,
                ContentType = file.ContentType
            };

        }

        public static PreviewResponseDTO ToPreview(
            this IFormFile file, 
            MemoryStream memoryStream)
        {
            return new PreviewResponseDTO
            {
                Name = Path.GetFileNameWithoutExtension(file.FileName),
                Content = memoryStream.ToArray(),
                Extension = Path.GetExtension(file.FileName),
                Size = memoryStream.Length,
                IsDirectory = false,
                ContentType = file.ContentType
            };
        }

        public static FileModel ToFileModelForCreate(
            this CreateEntryDTO file, 
            UserLoggedInfoDTO user,
            string path)
        {
            return new FileModel
            {
                Id = Guid.NewGuid(),
                Name = file.Name,
                Extension = file.Extension,
                IsDirectory = file.IsDirectory,
                ParentId = Guid.Parse(file.ParentId),
                ModifiedBy = user.UserDisplayName,
                ModifiedDate = DateTime.UtcNow,
                UserId = Guid.Parse(user.UserId),
                ContentType = file.ContentType,
                Path = path
            };
        }

        public static FileModel ToFileModelRename(
            this RenameEntryDTO file,
            FileModel oldFile,
            UserLoggedInfoDTO user)
        {
            return new FileModel
            {
                Id = Guid.Parse(file.FileId),
                Name = file.NewName,
                Extension = file.Extension,
                IsDirectory = oldFile.IsDirectory,
                ParentId = Guid.Parse(file.ParentId),
                ModifiedBy = user.UserDisplayName,
                ModifiedDate = DateTime.UtcNow,
                UserId = oldFile.UserId,
                Path = oldFile.Path
            };
        }

        public static FileResponseDTO ToFileResponse(
            this FileModel fileModel)
        {
            return new FileResponseDTO
            {
                Name = fileModel.Name,
                Content = fileModel.Content,
                Extension = fileModel.Extension,
                Size = fileModel.Size,
                IsDirectory = fileModel.IsDirectory,
                ContentType = fileModel.ContentType
            };
        }
    }
}
