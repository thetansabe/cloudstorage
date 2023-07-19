using FileExplorer.DataModel;
using FileExplorer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FileExplorer.API
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : BaseController<FileModel>
    {
        private readonly IStorageService _service;
        public FileController(IService<FileModel> baseService) : base(baseService)
        {
            _service = (IStorageService)baseService;
        }

        /// <summary>
        /// Create file or folder
        /// </summary>
        [HttpPost]
        [Authorize]
        [Route("Create")]
        public async Task<IActionResult> CreateFileAndFoldersAsync([FromBody] CreateEntryDTO info)
        {
            var user = HttpContext.Request.GetLoggedUser();
            var res = await _service.CreateFileAsync(info, user);

            if (!res.IsNullOrEmpty())
                return BadRequest(res);

            return Ok("New file created");

        }

        /// <summary>
        /// Rename file or folder
        /// </summary>
        [HttpPut]
        [Authorize]
        [Route("Rename")]
        public async Task<IActionResult> RenameFilesAndFoldersAsync(
            [FromBody] RenameEntryDTO info)
        {
            var user = HttpContext.Request.GetLoggedUser();
            string res = await _service.RenameAsync(info, user);

            if (res.IsNullOrEmpty())
                return Ok("Success! File renamed");

            return BadRequest(res);
        }

        /// <summary>
        /// View file or folder details
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("Details")]
        public async Task<IActionResult> GetFullPropertiesAsync([FromQuery] string fileId)
        {
            var user = HttpContext.Request.GetLoggedUser();
            var res = await _service.GetFullPropertiesAsync(fileId, user);

            if (res != null)
                return Ok(res);
            return BadRequest("FileId not found or Cannot access");
        }

        /// <summary>
        /// Delete many files or folders at once
        /// </summary>
        [HttpDelete]
        [Authorize]
        [Route("Delete")]
        public async Task<IActionResult> DeleteFilesAndFoldersAsync([FromBody] List<string> fileIds)
        {
            var user = HttpContext.Request.GetLoggedUser();
            var res = await _service.DeleteFilesAsync(fileIds, user);

            if (res)
                return Ok("Success! Files deleted");
            return BadRequest("Not all FileIds were deleted! Check for permission and make sure FileId correct");
        }

        /// <summary>
        /// Upload files from local device to server
        /// </summary>
        [HttpPost("Upload")]
        [Authorize]
        public async Task<IActionResult> Upload(
            List<IFormFile> files,
            [FromForm] string storageId)
        {
            var user = HttpContext.Request.GetLoggedUser();
            var res = await _service
                    .UploadFilesWithContentAsync(files, user.UserId, user.UserDisplayName, storageId);

            ProblemDetails response = new()
            {
                Title = "Failed to upload files",
                Detail = res,
                Status = StatusCodes.Status400BadRequest,
            };

            if (res.IsNullOrEmpty())
            {
                response.Title = "Success!";
                response.Status = StatusCodes.Status200OK;
                response.Detail = "Files uploaded";
                return Ok(response);
            }

            return BadRequest(response);
        }

        /// <summary>
        /// Download file server to local device
        /// </summary>
        [HttpGet("Download")]
        [Authorize]
        public async Task<IActionResult> DownloadAsync(
                       [FromQuery] string fileId)
        {
            var userId = HttpContext.Request.GetLoggedUser().UserId;
            var res = await _service.DownloadFileAsync(fileId, userId);

            if (res == null)
                return BadRequest("Failed! FileId not found or You cannot access");

            return File(
                res.Content, 
                contentType: res.ContentType, 
                fileDownloadName: Path.Combine(res.Name, res.Extension), 
                enableRangeProcessing: true);
        }

        /// <summary>
        /// Get files and folders in a path with limited number of items
        /// </summary>
        [HttpGet("List")]
        [Authorize]
        public async Task<IActionResult> ListWithPaginationAsync(
                       [FromQuery] string parentId,
                       [FromQuery] int page,
                       [FromQuery] int pageSize = 10
                   )
        {
            var userId = HttpContext.Request.GetLoggedUser().UserId;

            FilesPagedResponseDTO res = await _service.
                ListWithPaginationAsync(parentId, userId, page, pageSize);
            
            if (res == null)
                return BadRequest("Failed! Wrong userId or path");

            return Ok(res);
        }

        /// <summary>
        /// Preview files stored in database
        /// </summary>
        [HttpGet("Preview")]
        [Authorize]
        public async Task<IActionResult> PreviewFileAsync(
            [FromQuery] string fileId)
        {
            var userId = HttpContext.Request.GetLoggedUser().UserId;
            var res = await _service.PreviewFilesAsync(fileId, userId);
            
            if (res == null)
                return BadRequest("FileId not found or You cannot access");

            return File(
                res.Content,
                contentType: res.ContentType,
                fileDownloadName: Path.Combine(res.Name, res.Extension),
                enableRangeProcessing: true);
        }

        /// <summary>
        /// Move files or folders to another path.
        /// Also move sub files and folders if any.
        /// </summary>
        [HttpPut("Move")]
        [Authorize]
        public async Task<IActionResult> MoveStorageItemsAsync(
            [FromBody] MoveEntryDTO data
        )
        {
            var userId = HttpContext.Request.GetLoggedUser().UserId;
            var res = await _service.MoveStorageItemsAsync(data, userId);

            if (!res)
                return BadRequest("Not all files were moved! FileId not found or You cannot access");

            return Ok("Files moved successfully");
        }
    }
}
