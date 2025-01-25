namespace Clone_Backend_Twitter.Models.Response;

public class GetTweetResponse
{
    public class Tweet
    {
        public int Id { get; set; }
        public string? UserSlug { get; set; }
        public string Body { get; set; }
        public string? Image { get; set; }
        public DateTime CreatedAt { get; set; }
        public int AnswerOf { get; set; }
        public User User { get; set; }
        public List<Like> Likes { get; set; }
    }
    public class User
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string? Avatar { get; set; }
    }

    public class Like
    {
        public string UserSlug { get; set; }
    }
}

