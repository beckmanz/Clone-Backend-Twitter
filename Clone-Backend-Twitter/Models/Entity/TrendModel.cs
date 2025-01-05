namespace Clone_Backend_Twitter.Models.Entity;

public class TrendModel
{
    public int Id { get; set; }
    public string Hashtag { get; set; }
    public int Counter { get; set; } = 1;
    public DateTime UpdateAt { get; set; } = DateTime.Now;
}