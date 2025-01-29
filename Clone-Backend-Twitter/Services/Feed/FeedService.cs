using Clone_Backend_Twitter.Data;
using Clone_Backend_Twitter.Models.Entity;
using Clone_Backend_Twitter.Models.Response;
using Clone_Backend_Twitter.Utils;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

namespace Clone_Backend_Twitter.Services.Feed;

public class FeedService : IFeedInterface
{
    private readonly AppDbContext _context;
    private readonly Url _url;

    public FeedService(AppDbContext context, Url url)
    {
        _context = context;
        _url = url;
    }

    public async Task<ResponseModel<object>> GetFeed(UserModel User, int currentPage, int perPage)
    {
        ResponseModel<object> response = new ResponseModel<object>();
        try
        {
            var followings = await _context.Follows
                .Where(f => f.User1Slug == User.Slug)
                .Select(f => f.User2Slug)
                .ToListAsync();
            
            var tweets = await _context.Tweets
                .Where(t => followings.Contains(t.UserSlug) && t.AnswerOf == 0)
                .Include(t => t.User)
                .Include(t => t.Likes)
                .OrderByDescending(t => t.CreatedAt)
                .Skip(currentPage * perPage)
                .Take(perPage)
                .ToListAsync();
            
            var feed = new
            {
                perPage = perPage,
                Page = currentPage,
                Feed = tweets.Select(tweet => new GetTweetResponse.Tweet()
                {
                    Id = tweet.Id,
                    UserSlug = tweet.UserSlug,
                    Body = tweet.Body,
                    Image = tweet.Image,
                    CreatedAt = tweet.CreatedAt,
                    AnswerOf = tweet.AnswerOf,
                    User = new GetTweetResponse.User()
                    {
                        Name = tweet.User.Name,
                        Slug = tweet.User.Slug,
                        Avatar = _url.GetPublicUrl(tweet.User.Avatar)
                    },
                    Likes = tweet.Likes.Select(l => new GetTweetResponse.Like()
                    {
                        UserSlug = l.UserSlug,
                    }).ToList()
                }).ToList()
            };

            response.Status = true;
            response.Message = "Feed atualizado com sucesso";
            response.Data = feed;
            return response;
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
            response.Status = false;
            return response;
        }
    }
}