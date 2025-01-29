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
    public class SuggestionController : ControllerBase
    {
        private readonly IAuthInterface _authInterface;
        private readonly IUserInterface _userInterface;

        public SuggestionController(IAuthInterface authInterface, IUserInterface userInterface)
        {
            _authInterface = authInterface;
            _userInterface = userInterface;
        }
        [HttpGet]
        public async Task<ActionResult<ResponseModel<object>>> GetUserSuggestions()
        {
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
            
            var User = await _authInterface.VerifyJwt(token);
            if (User == null)
            {
                return Unauthorized("Acesso Negado!");
            }
            
            var response = await _userInterface.GetUserSuggestions(User);
            return Ok(response);
        }
    }
}
