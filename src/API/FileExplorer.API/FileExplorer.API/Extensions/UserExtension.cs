using FileExplorer.Service;
using System.IdentityModel.Tokens.Jwt;

namespace FileExplorer.API
{
    public static class UserExtension
    {
        public static UserLoggedInfoDTO GetLoggedUser(this HttpRequest request)
        {
            var authHeader = request.Headers["Authorization"];
            var token = authHeader.ToString().Split(" ")[1];

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            return new UserLoggedInfoDTO
            {
                UserId = jwtToken.Claims.First(claim => claim.Type == "userid").Value,
                UserDisplayName = jwtToken.Claims.First(claim => claim.Type == "displayname").Value,
                UserName = jwtToken.Claims.First(claim => claim.Type == "username").Value
            };
        }
    }
}
