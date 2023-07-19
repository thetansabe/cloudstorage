using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.DesktopApp
{
    public class DirectoryItem
    {
        public Image Icon { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public DateTime CreatedDate { get; set; }   
        public DateTime ModifiedDate { get; set; }
        public string? Size { get; set; }
        public string Path { get; set; } = null!;
    }
}
