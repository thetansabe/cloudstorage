using FileExplorer.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Service
{
    public class FilesPagedResponseDTO
    {
        public List<FileModel> Files { get; set; } = new();

        public int TotalCount { get; set; }

        public int CurrentPage { get; set; }
    }
}
