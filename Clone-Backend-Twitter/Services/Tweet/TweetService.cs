using System.Text.RegularExpressions;
using Clone_Backend_Twitter.Data;
using Clone_Backend_Twitter.Models.Dto;
using Clone_Backend_Twitter.Models.Entity;
using Clone_Backend_Twitter.Models.Response;
using Clone_Backend_Twitter.Services.Auth;
using Clone_Backend_Twitter.Services.Trend;
using Clone_Backend_Twitter.Utils;
using Microsoft.EntityFrameworkCore;

namespace Clone_Backend_Twitter.Services.Tweet;

public class TweetService : ITweetInterface
{
    private readonly AppDbContext _context;
    private readonly ITrendInterface _trendService;
    private readonly Url _url;

    public TweetService(AppDbContext context, ITrendInterface trendService)
    {
        _context = context;
        _trendService = trendService;
    }

    public async Task<ResponseModel<TweetModel>> AddTweet(UserModel user, TweetDto tweetDto)
    {
        ResponseModel<TweetModel> response = new ResponseModel<TweetModel>();
        try
        {
            if(tweetDto.Answer != null)
            {
                var tweetData = await _context.Tweets.FirstOrDefaultAsync(t => t.Id == tweetDto.Answer);
                
                if(tweetData == null)
                {
                    response.Message = "Tweet original não encontrado";
                    return response;
                }
            }

            var newTweet = new TweetModel()
            {
                UserSlug = user.Slug,
                User = user,
                Body = tweetDto.Body,
                AnswerOf = Convert.ToInt32(tweetDto.Answer),
            };
            
            var hashTags = Regex.Matches(tweetDto.Body, @"#[a-zA-Z0-9_]+");
            if (hashTags.Count > 0)
            {
                foreach (Match hashtag in hashTags)
                {
                    string tag = hashtag.Value;
                    if (tag.Length >= 2)
                    {
                        await _trendService.AddTrend(tag);
                    }
                }
            }
            
            _context.Add(newTweet);
            await _context.SaveChangesAsync();
            
            response.Message = "Tweet adicionado com sucesso";
            response.Data = newTweet;
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