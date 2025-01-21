using Clone_Backend_Twitter.Models.Dto;
using Clone_Backend_Twitter.Models.Entity;
using Clone_Backend_Twitter.Models.Response;
using Clone_Backend_Twitter.Services.Auth;
using Clone_Backend_Twitter.Services.Tweet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clone_Backend_Twitter.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TweetController : ControllerBase
    {
        private readonly ITweetInterface _tweetInterface;
        private readonly IAuthInterface _authInterface;

        public TweetController(ITweetInterface tweetInterface, IAuthInterface authInterface)
        {
            _tweetInterface = tweetInterface;
            _authInterface = authInterface;
        }

        
        [HttpPost("AddTweet")]
        public async Task<ActionResult<ResponseModel<TweetModel>>> AddTweet(TweetDto tweetDto)
        {
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
            
            var user = await _authInterface.VerifyJwt(token);
            if (user == null)
            {
                return Unauthorized("Acesso Negado!");
            }
            
            var response = await _tweetInterface.AddTweet(user, tweetDto);
            return Ok(response);
        }
    }
}
