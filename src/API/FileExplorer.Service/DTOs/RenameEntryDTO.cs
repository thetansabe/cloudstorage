using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Service
{
    public class RenameEntryDTO
    {
        public string FileId { get; set; } = null!;
        public string? Extension { get; set; }
        public string ParentId { get; set; } = null!;
        public string NewName { get; set; } = null!;
    }
}
