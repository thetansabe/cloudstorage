using FileExplorer.DataConnect;
using FileExplorer.DataModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Repo
{
    public class UserRepository : Repository<UserModel>, IUserRepository
    {
        public UserRepository(FileExplorerContext iContext) : base(iContext)
        {
        }

        public async Task<UserModel> GetUserByUsernameAsync(string username)
        {
            var user = await Context.Set<UserModel>()
                                    .FirstOrDefaultAsync(x => x.UserName == username);
            return user;
        }
    }
}
