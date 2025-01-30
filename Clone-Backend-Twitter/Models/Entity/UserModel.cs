using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Clone_Backend_Twitter.Models.Entity;
public class UserModel
{
    public string Slug { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Name { get; set; }
    public string Avatar { get; set; } = "Avatar/defaultAvatar.jpg";
    public string Cover { get; set; } = "Cover/defaultCover.jpg";
    public string? Bio { get; set; }
    public string? Link { get; set; }
    [JsonIgnore]
    public Collection<TweetModel> Tweets { get; set; }
    [JsonIgnore]
    public Collection<TweetLikeModel> Likes { get; set; }
}