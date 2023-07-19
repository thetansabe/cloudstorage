using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Service
{
    public class MoveEntryDTO
    {
        public List<string> FileIds { get; set; } = new List<string>();
        public string NewPathId { get; set; } = null!;
    }
}
