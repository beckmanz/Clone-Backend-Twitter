namespace Clone_Backend_Twitter.Utils;

public class Url
{
    private readonly IConfiguration _configuration;

    public Url(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetPublicUrl(string url)
    {
        string urlBase = _configuration["BaseURL"];
        string publicUrl = $"{urlBase}/{url}";
        
        return publicUrl;
    }
}