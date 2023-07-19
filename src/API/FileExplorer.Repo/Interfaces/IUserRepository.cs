using FileExplorer.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Repo
{
    public interface IUserRepository: IRepository<UserModel>
    {
        /// <summary>
        /// Get user by username returns full info of user
        /// </summary>
        Task<UserModel> GetUserByUsernameAsync(string username);

    }
}
