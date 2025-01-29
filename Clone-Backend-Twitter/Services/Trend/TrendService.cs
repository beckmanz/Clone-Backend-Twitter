using Clone_Backend_Twitter.Data;
using Clone_Backend_Twitter.Models.Entity;
using Clone_Backend_Twitter.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace Clone_Backend_Twitter.Services.Trend;

public class TrendService : ITrendInterface
{
    private readonly AppDbContext _context;

    public TrendService(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddTrend(string Tag)
    {
        var hs = await _context.Trends.FirstOrDefaultAsync(t => t.Hashtag == Tag);
        if (hs != null)
        {
            hs.Counter = hs.Counter + 1;
            hs.UpdateAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }
        else
        {
            var newTrend = new TrendModel()
            {
                Hashtag = Tag
            };
            _context.Add(newTrend);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<ResponseModel<object>> GetTrends()
    {
        ResponseModel<object> response = new ResponseModel<object>();
        try
        {
            var trends = await _context.Trends
                .Where(t => t.UpdateAt >= DateTime.UtcNow.AddHours(-24))
                .OrderByDescending(t => t.Counter)
                .Take(4)
                .Select(t => new
                {
                    t.Hashtag,
                    t.Counter,
                })
                .ToListAsync();
            
            response.Status = true;
            response.Message = "Trends retornadas com sucesso";
            response.Data = trends;
            return response;
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
            response.Status = false;
            return response;
        }
    }
}