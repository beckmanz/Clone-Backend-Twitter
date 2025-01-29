using Clone_Backend_Twitter.Models.Dto;
using Clone_Backend_Twitter.Models.Entity;
using Clone_Backend_Twitter.Models.Response;

namespace Clone_Backend_Twitter.Services.User;

public interface IUserInterface
{
    Task<ResponseModel<object>> GetUser(string Slug);
    Task<ResponseModel<object>> GetUserTweets(string Slug, int currentPage, int perPage);
    Task<ResponseModel<object>> FollowToggle(UserModel User, string Slug);
    Task<ResponseModel<object>> UpdateUser(UserModel User, UpdateUserDto update);
    Task<ResponseModel<object>> GetUserSuggestions(UserModel User);
}