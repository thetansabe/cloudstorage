using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Service
{
    public class FileEntryDTO
    {
        public string? FileId { get; set; }
        public string Path { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public string? UserDisplayName { get; set; }
        
        public bool IsDirectory { get; set; }
        public string? Extension { get; set; }
        public string? ParentId { get; set; }
        public byte[]? Content { get; set; }
    }
}
