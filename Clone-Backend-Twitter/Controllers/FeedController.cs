using Clone_Backend_Twitter.Models.Response;
using Clone_Backend_Twitter.Services.Auth;
using Clone_Backend_Twitter.Services.Feed;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clone_Backend_Twitter.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FeedController : ControllerBase
    {
        private readonly IFeedInterface _feedInterface;
        private readonly IAuthInterface _authInterface;

        public FeedController(IFeedInterface feedInterface, IAuthInterface authInterface)
        {
            _feedInterface = feedInterface;
            _authInterface = authInterface;
        }
        
        [HttpGet]
        public async Task<ActionResult<ResponseModel<object>>> GetAnswers(int currentPage=0, int perPage=10)
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
            
            var response = await _feedInterface.GetFeed(User, currentPage, perPage);
            return Ok(response);
        }
    }
}
