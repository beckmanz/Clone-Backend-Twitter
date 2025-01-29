using Clone_Backend_Twitter.Models.Entity;
using Clone_Backend_Twitter.Models.Response;

namespace Clone_Backend_Twitter.Services.Feed;

public interface IFeedInterface
{
    Task<ResponseModel<object>> GetFeed(UserModel User, int currentPage, int perPage);
}