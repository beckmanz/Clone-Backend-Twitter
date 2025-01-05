using Clone_Backend_Twitter.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clone_Backend_Twitter.Data.Map;

public class TweetMap : IEntityTypeConfiguration<TweetModel>
{
    public void Configure(EntityTypeBuilder<TweetModel> builder)
    {
        builder.HasKey(t => t.Id);
        builder.HasOne(t => t.User).WithMany(u => u.Tweets).HasForeignKey(t => t.UserSlug).OnDelete(DeleteBehavior.NoAction);;
        builder.HasMany(t => t.Likes).WithOne(u => u.Tweet).HasForeignKey(t => t.TweetId).OnDelete(DeleteBehavior.NoAction);;
    }
}    