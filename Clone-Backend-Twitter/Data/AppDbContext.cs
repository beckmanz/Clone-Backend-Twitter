using Clone_Backend_Twitter.Data.Map;
using Clone_Backend_Twitter.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace Clone_Backend_Twitter.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
}