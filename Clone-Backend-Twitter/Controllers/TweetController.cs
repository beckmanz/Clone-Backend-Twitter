using Clone_Backend_Twitter.Models.Dto;
using Clone_Backend_Twitter.Models.Entity;
using Clone_Backend_Twitter.Models.Response;
using Clone_Backend_Twitter.Services.Auth;
using Clone_Backend_Twitter.Services.Tweet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

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
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ResponseModel<TweetModel>>> AddTweet([FromForm] string Body, [FromForm] int? Answer, IFormFile? Image)
        {
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
            
            var user = await _authInterface.VerifyJwt(token);
            if (user == null)
            {
                return Unauthorized("Acesso Negado!");
            }
            var tweetDto = new TweetDto()
            {
                Body = Body,
                Answer = Answer
            };
            var response = await _tweetInterface.AddTweet(user, tweetDto, Image);
            return Ok(response);
        }
        [HttpGet("GetTweet/{Id}")]
        public async Task<ActionResult<ResponseModel<GetTweetResponse.Tweet>>> GetTweet(int Id)
        {
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
            
            var user = await _authInterface.VerifyJwt(token);
            if (user == null)
            {
                return Unauthorized("Acesso Negado!");
            }
            
            var response = await _tweetInterface.GetTweet(Id);
            return Ok(response);
        }
        [HttpGet("GetAnswers/{Id}/Answers")]
        public async Task<ActionResult<ResponseModel<List<GetTweetResponse.Tweet>>>> GetAnswers(int Id)
        {
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
            
            var user = await _authInterface.VerifyJwt(token);
            if (user == null)
            {
                return Unauthorized("Acesso Negado!");
            }
            
            var response = await _tweetInterface.GetAnswers(Id);
            return Ok(response);
        }
        [HttpPost("{Id}/Like")]
        public async Task<ActionResult<ResponseModel<TweetLikeModel>>> LikeToggle(int Id)
        {
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
            
            var User = await _authInterface.VerifyJwt(token);
            if (User == null)
            {
                return Unauthorized("Acesso Negado!");
            }
            
            var response = await _tweetInterface.LikeToggle(User, Id);
            return Ok(response);
        }
    }
}
