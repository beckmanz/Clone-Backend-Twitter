using Clone_Backend_Twitter.Models.Response;

namespace Clone_Backend_Twitter.Services.Search;

public interface ISearchInterface
{
    Task<ResponseModel<object>> SearchTweet(string input, int currentPage, int perPage);
}