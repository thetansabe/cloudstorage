using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Service
{
    public class BinaryFileDTO
    {
        public MemoryStream MemoryStream { get; set; } = null!;
        public string StoragePath { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string UserDisplayName { get; set; } = null!;
        public string StorageId { get; set; } = null!;
    }
}
