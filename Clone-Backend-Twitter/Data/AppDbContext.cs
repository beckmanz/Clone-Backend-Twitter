using Clone_Backend_Twitter.Data.Map;
using Clone_Backend_Twitter.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace Clone_Backend_Twitter.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<UserModel> Users { get; set; }
    public DbSet<TweetModel> Tweets { get; set; }
    public DbSet<TweetLikeModel> Likes { get; set; }
    public DbSet<FollowModel> Follows { get; set; }
    public DbSet<TrendModel> Trends { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserMap());
        modelBuilder.ApplyConfiguration(new TweetMap());
        modelBuilder.ApplyConfiguration(new TweetLikeMap());
        base.OnModelCreating(modelBuilder);
    }
}