using Swashbuckle.AspNetCore.Annotations;

namespace Clone_Backend_Twitter.Models.Dto;

public class TweetDto()
{
    [SwaggerIgnore]
    public string? Body { get; set; }
    [SwaggerIgnore]
    public int? Answer { get; set; }
}