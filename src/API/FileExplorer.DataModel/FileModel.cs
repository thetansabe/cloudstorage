using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.DataModel
{
    [Table("File", Schema = "File")]
    public class FileModel : BaseModel
    {
        public string Path { get; set; } = null!;

        public string Name { get; set; } = null!;

        public long? Size { get; set; }

        /// <summary>
        /// If this is a folder, IsDirectory is true. 
        /// If this is a file, IsDirectory is false.
        /// </summary>
        public bool IsDirectory { get; set; }
        public string? ContentType { get; set; }

        public string? Extension { get; set; }

        public byte[]? Content { get; set; }

        [ForeignKey("UserId")]
        [Required]
        public Guid UserId { get; set; }
        public UserModel User { get; set; } = null!;

        [ForeignKey("ParentId")]
        public Guid? ParentId { get; set; }
        public FileModel? Parent { get; set; }
    }
}
