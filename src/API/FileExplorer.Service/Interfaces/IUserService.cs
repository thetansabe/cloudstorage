using FileExplorer.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Service
{
    public interface IUserService : IService<UserModel>
    {
        Task<UserModel> GetUserByUsernameAsync(string username);
        Task<bool> RegisterUserAsync(UserEntryDTO info);
        Task<UserModel> VerifyUserAsync(string username, string password);
        string GenerateJWT(JWTSettingDTO setting, UserModel user);
    }
}
