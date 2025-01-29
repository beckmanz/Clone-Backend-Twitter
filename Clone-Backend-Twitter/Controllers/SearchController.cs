using Clone_Backend_Twitter.Models.Response;
using Clone_Backend_Twitter.Services.Auth;
using Clone_Backend_Twitter.Services.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clone_Backend_Twitter.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchInterface _searchInterface;
        private readonly IAuthInterface _authInterface;

        public SearchController(ISearchInterface searchInterface, IAuthInterface authInterface)
        {
            _searchInterface = searchInterface;
            _authInterface = authInterface;
        }
        
        [HttpGet]
        public async Task<ActionResult<ResponseModel<object>>> SearchTweet(string input, int currentPage=0, int perPage=10)
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
            
            var response = await _searchInterface.SearchTweet(input, currentPage, perPage);
            return Ok(response);
        }
    }
}
