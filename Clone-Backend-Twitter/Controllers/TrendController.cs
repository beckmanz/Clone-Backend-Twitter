using Clone_Backend_Twitter.Models.Response;
using Clone_Backend_Twitter.Services.Auth;
using Clone_Backend_Twitter.Services.Trend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clone_Backend_Twitter.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TrendController : ControllerBase
    {
        private readonly ITrendInterface _trendInterface;
        private readonly IAuthInterface _authInterface;

        public TrendController(ITrendInterface trendInterface, IAuthInterface authInterface)
        {
            _trendInterface = trendInterface;
            _authInterface = authInterface;
        }
        [HttpGet]
        public async Task<ActionResult<ResponseModel<object>>> GetTrends()
        {
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
            
            var user = await _authInterface.VerifyJwt(token);
            if (user == null)
            {
                return Unauthorized("Acesso Negado!");
            }

            var response = await _trendInterface.GetTrends();
            return Ok(response);
        }
    }
}
