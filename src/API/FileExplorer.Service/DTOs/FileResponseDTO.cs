using FileExplorer.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Service
{
    public class FileResponseDTO
    {
        public string Name { get; set; } = null!;

        public string Extension { get; set; } = null!;

        public long? Size { get; set; }

        public byte[] Content { get; set; } = null!;
        public string ContentType { get; set; } = null!;

        public bool IsDirectory { get; set; }
    }
}
