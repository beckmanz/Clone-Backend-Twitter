using System.Diagnostics.Metrics;
using Clone_Backend_Twitter.Data;
using Clone_Backend_Twitter.Models.Dto;
using Clone_Backend_Twitter.Models.Entity;
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
            
            var tweets = await _context.Tweets
                .Where(t => t.UserSlug == user.Slug && t.AnswerOf == 0)
                .Include(t => t.Likes)
                .OrderByDescending(t => t.CreatedAt)
                .Skip(currentPage * perPage)
                .Take(perPage)
                .ToListAsync();

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

    public async Task<ResponseModel<object>> FollowToggle(UserModel User, string Slug)
    {
        ResponseModel<object> response = new ResponseModel<object>();
        try
        {
            if (User.Slug == Slug)
            {
                response.Status = false;
                response.Message = "Você não pode seguir a si mesmo!";
                return response;
            }
            var userFollow = await _context.Users.FirstOrDefaultAsync(u => u.Slug == Slug);
            if (userFollow == null)
            {
                response.Status = false;
                response.Message = "Usuário inexistente";
                return response;
            }
            
            var isFollow = await _context.Follows
                .Where(f => f.User1Slug == User.Slug && f.User2Slug == userFollow.Slug)
                .FirstOrDefaultAsync();

            if (isFollow != null)
            {
                _context.Remove(isFollow);
                await _context.SaveChangesAsync();
                
                response.Message = "Você deixou de seguir este usuário.";
                response.Data = new { Following = false };
            }
            else
            {
                var newFollowing = new FollowModel()
                {
                    User1Slug = User.Slug,
                    User2Slug = userFollow.Slug,
                };
                
                _context.Add(newFollowing);
                await _context.SaveChangesAsync();

                response.Message = "Você começou a seguir este usuário.";
                response.Data = new { Following = true };
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

    public async Task<ResponseModel<object>> UpdateUser(UserModel User, UpdateUserDto update)
    {
        ResponseModel<object> response = new ResponseModel<object>();
        try
        {
            if (!string.IsNullOrWhiteSpace(update.Name))
            {
                User.Name = update.Name;
            }
            if (!string.IsNullOrWhiteSpace(update.Bio))
            {
                User.Bio = update.Bio;
            }
            if (!string.IsNullOrWhiteSpace(update.Link))
            {
                User.Link = update.Link;
            }
            
            _context.Update(User);
            await _context.SaveChangesAsync();
            
            var userData = new
            {
                User.Name,
                User.Slug,
                Avatar = _url.GetPublicUrl(User.Avatar),
                Cover = _url.GetPublicUrl(User.Cover),
                User.Bio,
                User.Link,
            };
            
            response.Message = "Usuário atualizado com sucesso";
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

    public async Task<ResponseModel<object>> GetUserSuggestions(UserModel User)
    {
        ResponseModel<object> response = new ResponseModel<object>();
        try
        {
            var followings = await _context.Follows
                .Where(f => f.User1Slug == User.Slug)
                .Select(f => f.User2Slug)
                .ToListAsync();
            
            var suggestions = await _context.Users
                .Where(u => u.Slug != User.Slug && !followings.Contains(u.Slug))
                .OrderBy(u => Guid.NewGuid())
                .Take(2)
                .Select( u => new
                {
                    u.Name,
                    u.Slug,
                    Avatar = _url.GetPublicUrl(u.Avatar),
                })
                .ToListAsync();
            
            response.Message = "Sugestões buscadas com sucesso";
            response.Data = suggestions;
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