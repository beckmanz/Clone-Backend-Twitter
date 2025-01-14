using Clone_Backend_Twitter.Models.Dto;
using Clone_Backend_Twitter.Models.Entity;
using Clone_Backend_Twitter.Models.Response;
using Clone_Backend_Twitter.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clone_Backend_Twitter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthInterface _authInterface;

        public AuthController(IAuthInterface authInterface)
        {
            _authInterface = authInterface;
        }

        [HttpPost("signup")]
        public async Task<ActionResult<ResponseModel<AuthResponse>>> Signup(SignupDto signupDto)
        {
            var response = await _authInterface.Signup(signupDto);
            return Ok(response);
        }
        [HttpPost("signin")]
        public async Task<ActionResult<ResponseModel<AuthResponse>>> Signin(SigninDto signinDto)
        {
            var response = await _authInterface.Signin(signinDto);
            return Ok(response);
        }
    }
}
