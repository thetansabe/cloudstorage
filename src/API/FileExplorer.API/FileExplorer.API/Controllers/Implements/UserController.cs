using FileExplorer.DataModel;
using FileExplorer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FileExplorer.API
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : BaseController<UserModel>
    {
        private readonly IUserService _service;
        private readonly IConfiguration _config;

        public UserController(IService<UserModel> baseService, IConfiguration iConfig) : base(baseService)
        {
            _service = (IUserService)baseService;
            _config = iConfig;
        }

        /// <summary>
        /// Login with username and password
        /// </summary>
        /// <response code="200">Success! Returns JWT token and user info</response>
        /// <response code="401">Unauthorized! Wrong username or password</response>
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginAsync(UserLoginDTO userInfo)
        {
            var res = await _service.VerifyUserAsync(userInfo.UserName, userInfo.Password);

            JWTSettingDTO jWTSetting = new()
            {
                SecretKey = _config.GetSection("Jwt")["SecretKey"],
                Expire = int.Parse(_config.GetSection("Jwt")["ExpireDays"]),
                Issuer = _config.GetSection("Jwt")["Issuer"],
                Audience = _config.GetSection("Jwt")["Audience"]
            };

            if (res == null)
                return Unauthorized();

            var returnResult = new LoginResponseDTO() {
                Token = _service.GenerateJWT(jWTSetting, res),
                User = res
            };

            return Ok(returnResult);
        }

        /// <summary>
        /// Register new account
        /// </summary>
        /// <response code="200">Success! New account created</response>
        /// <response code="400">Bad request! Wrong username or password</response>
        [HttpPost]
        [Route("Register")]
        [RegisterExceptionFilter]
        public async Task<IActionResult> RegisterAsync([FromBody] UserEntryDTO info)
        {
                var res = await _service.RegisterUserAsync(info);

                if (res)
                {
                    return Ok("New account created");
                }

                ProblemDetails problemDetails = new()
                {
                    Title = "Register failed!",
                    Detail = "Username existed",
                    Status = 400,
                };

                return BadRequest(problemDetails);
        }
    }

}
