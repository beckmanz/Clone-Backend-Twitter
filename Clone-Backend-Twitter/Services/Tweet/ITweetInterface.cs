using Clone_Backend_Twitter.Models.Dto;
using Clone_Backend_Twitter.Models.Entity;
using Clone_Backend_Twitter.Models.Response;

namespace Clone_Backend_Twitter.Services.Tweet;

public interface ITweetInterface
{
    Task<ResponseModel<TweetModel>> AddTweet(UserModel user, TweetDto tweetDto);
    Task<ResponseModel<GetTweetResponse.Tweet>> GetTweet(int Id);
    Task<ResponseModel<List<GetTweetResponse.Tweet>>> GetAnswers(int Id);
}