using Clone_Backend_Twitter.Models.Dto;
using Clone_Backend_Twitter.Models.Entity;
using Clone_Backend_Twitter.Models.Response;
using Clone_Backend_Twitter.Services.Auth;
using Clone_Backend_Twitter.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clone_Backend_Twitter.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserInterface _userInterface;
        private readonly IAuthInterface _authInterface;

        public UserController(IUserInterface userInterface, IAuthInterface authInterface)
        {
            _userInterface = userInterface;
            _authInterface = authInterface;
        }

        [HttpGet("{Slug}")]
        public async Task<ActionResult<ResponseModel<object>>> GetUser(string Slug)
        {
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
            
            var User = await _authInterface.VerifyJwt(token);
            if (User == null)
            {
                return Unauthorized("Acesso Negado!");
            }
            
            var response = await _userInterface.GetUser(Slug);
            return Ok(response);
        }
        [HttpGet("{Slug}/tweets")]
        public async Task<ActionResult<ResponseModel<object>>> GetUser(string Slug, int currentPage=0, int perPage=10)
        {
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
            
            var User = await _authInterface.VerifyJwt(token);
            if (User == null)
            {
                return Unauthorized("Acesso Negado!");
            }

            if (perPage > 30)
            {
                return BadRequest("Valores de página e tamanho inválidos");
            }
            
            var response = await _userInterface.GetUserTweets(Slug, currentPage, perPage);
            return Ok(response);
        }
        [HttpPost("{Slug}/follow")]
        public async Task<ActionResult<ResponseModel<object>>> FolllowToggle(string Slug)
        {
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
            
            var User = await _authInterface.VerifyJwt(token);
            if (User == null)
            {
                return Unauthorized("Acesso Negado!");
            }
            
            var response = await _userInterface.FollowToggle(User, Slug);
            return Ok(response);
        }
        [HttpPut]
        public async Task<ActionResult<ResponseModel<object>>> UpdateUser(UpdateUserDto update)
        {
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
            
            var User = await _authInterface.VerifyJwt(token);
            if (User == null)
            {
                return Unauthorized("Acesso Negado!");
            }
            
            var response = await _userInterface.UpdateUser(User, update);
            return Ok(response);
        }
        [HttpPut("Avatar")]
        public async Task<ActionResult<ResponseModel<object>>> UpdateAvatar(IFormFile? Avatar)
        {
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
            
            var User = await _authInterface.VerifyJwt(token);
            if (User == null)
            {
                return Unauthorized("Acesso Negado!");
            }
            
            var response = await _userInterface.UpdateAvatar(User, Avatar);
            return Ok(response);
        }
        [HttpPut("Cover")]
        public async Task<ActionResult<ResponseModel<object>>> UpdateCover(IFormFile? Cover)
        {
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
            
            var User = await _authInterface.VerifyJwt(token);
            if (User == null)
            {
                return Unauthorized("Acesso Negado!");
            }
            
            var response = await _userInterface.UpdateCover(User, Cover);
            return Ok(response);
        }
    }
}
