using Clone_Backend_Twitter.Data;
using Clone_Backend_Twitter.Models.Response;
using Clone_Backend_Twitter.Utils;
using Microsoft.EntityFrameworkCore;

namespace Clone_Backend_Twitter.Services.Search;

public class SearchService : ISearchInterface
{
    private readonly AppDbContext _context;
    private readonly Url _url;

    public SearchService(AppDbContext context, Url url)
    {
        _context = context;
        _url = url;
    }
    public async Task<ResponseModel<object>> SearchTweet(string input, int currentPage, int perPage)
    {
        ResponseModel<object> response = new ResponseModel<object>();
        try
        {
            var tweets = await _context.Tweets
                .Where(t => t.Body.ToLower().Contains(input.ToLower()) && t.AnswerOf == 0)
                .Include(t => t.User)
                .Include(t => t.Likes)
                .OrderByDescending(t => t.CreatedAt)
                .Skip(currentPage * perPage)
                .Take(perPage)
                .ToListAsync();
            
            var search = new
            {
                perPage = perPage,
                Page = currentPage,
                Search = tweets.Select(tweet => new GetTweetResponse.Tweet()
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
            response.Message = "Busca realizada com sucesso";
            response.Data = search;
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