using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Clone_Backend_Twitter.Models.Entity;

public class TweetModel
{
    public int Id { get; set; }
    public string UserSlug { get; set; }
    [JsonIgnore]
    public UserModel User { get; set; }
    public string Body { get; set; }
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int AnswerOf { get; set; } = 0;
    [JsonIgnore]
    public Collection<TweetLikeModel> Likes { get; set; }
}