using System.Text.Json.Serialization;

namespace Clone_Backend_Twitter.Models.Entity;

public class TweetLikeModel
{
    public int Id { get; set; }
    public string UserSlug { get; set; }
    [JsonIgnore]
    public UserModel User { get; set; }
    public int TweetId { get; set; }
    public TweetModel Tweet { get; set; }
}