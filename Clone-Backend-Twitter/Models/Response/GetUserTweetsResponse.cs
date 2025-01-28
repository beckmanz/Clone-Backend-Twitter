namespace Clone_Backend_Twitter.Models.Response;

public class GetUserTweetsResponse
{
    public int Id { get; set; }
    public string UserSlug { get; set; }
    public string Body { get; set; }
    public string Image { get; set; }
    public DateTime CreatedAt { get; set; }
    public int AnswerOf { get; set; }
    public List<Like> Likes { get; set; }
}

public class Like()
{
    public string UserSlug { get; set; }
}