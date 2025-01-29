using Clone_Backend_Twitter.Models.Response;

namespace Clone_Backend_Twitter.Services.Trend;

public interface ITrendInterface
{
    Task AddTrend(string Tag);
    Task<ResponseModel<object>> GetTrends();
}