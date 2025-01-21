using Clone_Backend_Twitter.Data;
using Clone_Backend_Twitter.Models.Entity;
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
}