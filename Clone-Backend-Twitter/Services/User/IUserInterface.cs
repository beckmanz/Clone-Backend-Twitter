using Clone_Backend_Twitter.Models.Entity;
using Clone_Backend_Twitter.Models.Response;

namespace Clone_Backend_Twitter.Services.User;

public interface IUserInterface
{
    Task<ResponseModel<object>> GetUser(string Slug);
    Task<ResponseModel<object>> GetUserTweets(string Slug, int currentPage, int perPage);
}