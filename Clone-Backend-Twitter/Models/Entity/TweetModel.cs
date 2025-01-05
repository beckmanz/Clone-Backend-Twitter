using System.Collections.ObjectModel;

namespace Clone_Backend_Twitter.Models.Entity;

public class TweetModel
{
    public int Id { get; set; }
    public string UserSlug { get; set; }
    public UserModel User { get; set; }
    public string Body { get; set; }
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int AnswerOf { get; set; } = 0;
    public Collection<TweetLikeModel> Likes { get; set; }
}