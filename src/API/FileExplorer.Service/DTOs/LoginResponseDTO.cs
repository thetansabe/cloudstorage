using FileExplorer.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Service
{
    public class LoginResponseDTO
    {
        public UserModel User { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
