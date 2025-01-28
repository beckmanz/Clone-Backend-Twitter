using System.Diagnostics.Metrics;
using Clone_Backend_Twitter.Data;
using Clone_Backend_Twitter.Models.Response;
using Clone_Backend_Twitter.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Clone_Backend_Twitter.Services.User;

public class UserService : IUserInterface
{
    private readonly AppDbContext _context;
    private readonly Url _url;

    public UserService(AppDbContext context, Url url)
    {
        _context = context;
        _url = url;
    }

    public async Task<ResponseModel<object>> GetUser(string Slug)
    {
        ResponseModel<object> response = new ResponseModel<object>();
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Slug == Slug);
            if (user == null)
            {
                response.Message = "Usuário inexistente";
                response.Status = false;
                return response;
            }
            
            var followingCount = await _context.Follows.Where(f => f.User1Slug == user.Slug).CountAsync();
            var followersCount = await _context.Follows.Where(f => f.User2Slug == user.Slug).CountAsync();
            var tweetCount = await _context.Tweets.Where(t => t.UserSlug == user.Slug).CountAsync();

            var userData = new
            {
                user.Name,
                user.Slug,
                Avatar = _url.GetPublicUrl(user.Avatar),
                Cover = _url.GetPublicUrl(user.Cover),
                user.Bio,
                user.Link,
                followingCount,
                followersCount,
                tweetCount,
            };
            
            response.Message = "Usuário coletado com sucesso";
            response.Data = userData;
            return response;

        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
            response.Status = false;
            return response;
        }
    }

    public async Task<ResponseModel<object>> GetUserTweets(string Slug, int currentPage, int perPage)
    {
        ResponseModel<object> response = new ResponseModel<object>();
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Slug == Slug);
            if (user == null)
            {
                response.Message = "Usuário inexistente";
                response.Status = false;
                return response;
            }
            
            var tweets = _context.Tweets
                .Where(t => t.UserSlug == user.Slug && t.AnswerOf == 0)
                .Include(t => t.Likes)
                .OrderByDescending(t => t.CreatedAt)
                .Skip(currentPage * perPage)
                .Take(perPage)
                .ToList();

            if (tweets.Count == 0)
            {
                response.Message = "Usuário não tem tweets";
                response.Status = false;
                return response;
            }

            var tweetData = new 
            {
                perPage,
                Page = currentPage,
                Tweets = tweets.Select(tweet => new GetUserTweetsResponse()
                {
                    Id = tweet.Id,
                    UserSlug = tweet.UserSlug,
                    Body = tweet.Body,
                    Image = tweet.Image,
                    CreatedAt = tweet.CreatedAt,
                    AnswerOf = tweet.AnswerOf,
                    Likes = tweet.Likes.Select(l => new Like()
                    {
                        UserSlug = l.UserSlug,
                    }).ToList()
                }).ToList()
            };

            response.Message = "Tweets coletados com sucesso";
            response.Data = tweetData;
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