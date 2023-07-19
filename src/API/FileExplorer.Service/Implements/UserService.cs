using FileExplorer.DataModel;
using FileExplorer.Repo;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Service
{
    public class UserService : Service<UserModel>, IUserService
    {
        private readonly IUserRepository _repository;
        public UserService(IRepository<UserModel> repository) : base(repository)
        {
            _repository = (IUserRepository)repository;
        }

        public async Task<UserModel> GetUserByUsernameAsync(string username)
        {
            return await _repository.GetUserByUsernameAsync(username);
        }

        public async Task<bool> RegisterUserAsync(UserEntryDTO info)
        {
            var user = info.ToUserModel();

            var userName = await GetUserByUsernameAsync(user.UserName);

            if(userName != null) return false;

            user.HashPassword = BCrypt.Net.BCrypt.HashPassword(user.HashPassword);

            await _repository.AddAsync(user);

            return true;
        }

        public async Task<UserModel> VerifyUserAsync(string username, string password)
        {
            var user = await GetUserByUsernameAsync(username);

            if(user == null) return null;

            if(BCrypt.Net.BCrypt.Verify(password, user.HashPassword))
                return user;
            return null;

        }

        public string GenerateJWT(JWTSettingDTO setting, UserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(setting.SecretKey);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new("userid", user.Id.ToString()),
                new("username", user.UserName),
                new("displayname", user.DisplayName),
                new("email", user.Email),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.UtcNow.AddDays(setting.Expire),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = setting.Issuer,
                Audience = setting.Audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);
            return jwt;
        }
    }
}
