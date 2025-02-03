using System.Text.RegularExpressions;
using Clone_Backend_Twitter.Data;
using Clone_Backend_Twitter.Models.Dto;
using Clone_Backend_Twitter.Models.Entity;
using Clone_Backend_Twitter.Models.Response;
using Clone_Backend_Twitter.Services.Trend;
using Clone_Backend_Twitter.Utils;
using Microsoft.EntityFrameworkCore;

namespace Clone_Backend_Twitter.Services.Tweet;

public class TweetService : ITweetInterface
{
    private readonly AppDbContext _context;
    private readonly ITrendInterface _trendService;
    private readonly Url _url;

    public TweetService(AppDbContext context, ITrendInterface trendService, Url url)
    {
        _context = context;
        _trendService = trendService;
        _url = url;
    }

    public async Task<ResponseModel<TweetModel>> AddTweet(UserModel user, TweetDto tweetDto, IFormFile? Image)
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
                    response.Status = false;
                    return response;
                }
            }
            
            string? imageUrl = null;
            if (Image is not null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(Image.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    response.Message = "Formato de imagem inválido. Apenas .jpg, .jpeg e .png são permitidos.";
                    response.Status = false;
                    return response;
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Tweet");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = $"{Guid.NewGuid()}_Tweet{fileExtension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(fileStream);
                }

                imageUrl = _url.GetPublicUrl(Path.Combine($"Tweet/{fileName}"));
            }

            var newTweet = new TweetModel()
            {
                UserSlug = user.Slug,
                User = user,
                Body = tweetDto.Body,
                Image = imageUrl,
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
    public async Task<ResponseModel<GetTweetResponse.Tweet>> GetTweet(int Id)
    {
        ResponseModel<GetTweetResponse.Tweet> response = new ResponseModel<GetTweetResponse.Tweet>();
        try
        {
            var tweet = await _context.Tweets
                .Include(t => t.User)
                .Include(t => t.Likes)
                .FirstOrDefaultAsync(t => t.Id == Id);

            if (tweet == null)
            {
                response.Status = false;
                response.Message = "Tweet não encontrado";
                return response;
            }
            
            var tweetData = new GetTweetResponse.Tweet()
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
            };
            
            response.Status = true;
            response.Message = "Tweet encontrado com sucesso";
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
    public async Task<ResponseModel<List<GetTweetResponse.Tweet>>> GetAnswers(int Id)
    {
        ResponseModel<List<GetTweetResponse.Tweet>> response = new ResponseModel<List<GetTweetResponse.Tweet>>();
        try
        {
            var tweets = await _context.Tweets
                .Include(t => t.User)
                .Include(t => t.Likes)
                .Where(t => t.AnswerOf == Id)
                .ToListAsync();

            if (tweets == null || tweets.Count == 0)
            {
                response.Status = false;
                response.Message = "Tweet inexistente ou sem respostas";
                return response;
            }

            var tweetData = tweets.Select(tweet => new GetTweetResponse.Tweet()
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
            }).ToList();
            
            response.Status = true;
            response.Message = "Respostas encontradas com sucesso";
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

    public async Task<ResponseModel<TweetLikeModel>> LikeToggle(UserModel User, int Id)
    {
        ResponseModel<TweetLikeModel> response = new ResponseModel<TweetLikeModel>();
        try
        {
            var tweet = await _context.Tweets
                .FirstOrDefaultAsync(t => t.Id == Id);
            if (tweet == null)
            {
                response.Status = false;
                response.Message = "Tweet inexistente";
                return response;
            }

            var isLiked = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserSlug == User.Slug && l.TweetId == Id);
            if (isLiked != null)
            {
                _context.Remove(isLiked);
                await _context.SaveChangesAsync();
                
                response.Data = null;
                response.Message = "Like removido com sucesso";
            }
            else
            {
                var newLike = new TweetLikeModel()
                {
                    UserSlug = User.Slug,
                    User = User,
                    TweetId = Id,
                    Tweet = tweet,
                };
                _context.Add(newLike);
                await _context.SaveChangesAsync();

                response.Data = null;
                response.Message = "Like adicionado com sucesso!";
            }
            
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