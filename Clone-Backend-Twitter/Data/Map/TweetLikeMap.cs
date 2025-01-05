using Clone_Backend_Twitter.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clone_Backend_Twitter.Data.Map;

public class TweetLikeMap : IEntityTypeConfiguration<TweetLikeModel>
{
    public void Configure(EntityTypeBuilder<TweetLikeModel> builder)
    {
        builder.HasKey(t => t.Id);
        builder.HasOne(t => t.User).WithMany(u => u.Likes).HasForeignKey(t => t.UserSlug).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(t => t.Tweet).WithMany(t => t.Likes).HasForeignKey(t => t.TweetId).OnDelete(DeleteBehavior.NoAction);
    }
}    