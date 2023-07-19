using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Service
{
    public class CreateEntryDTO
    {
        public string Name { get; set; } = null!;
        public bool IsDirectory { get; set; }
        public string Extension { get; set; } = null!;
        public string ParentId { get; set; } = null!;
        public string ContentType { get; set; } = null!;
    }
}
