using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Service
{
    public class UserLoggedInfoDTO
    {
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string UserDisplayName { get; set; } = null!;
    }
}
