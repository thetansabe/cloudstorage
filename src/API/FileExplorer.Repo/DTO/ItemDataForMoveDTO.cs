using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Repo
{
    public class ItemDataForMoveDTO
    {
        public IQueryable<string> SubFileIds { get; set; } = null!;
        public string NewPath { get; set; } = null!;
        public string NewParentId { get; set; } = null!;
    }
}
