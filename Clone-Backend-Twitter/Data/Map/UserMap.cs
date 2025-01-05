using Clone_Backend_Twitter.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clone_Backend_Twitter.Data.Map;

public class UserMap : IEntityTypeConfiguration<UserModel>
{
    public void Configure(EntityTypeBuilder<UserModel> builder)
    {
        builder.HasKey(u => u.Slug);
        builder.Property(u => u.Slug).IsRequired();

        builder.Property(u => u.Email).IsRequired();
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.Name).IsRequired();
        
        builder.HasMany(u => u.Tweets).WithOne(t => t.User).HasForeignKey(t => t.UserSlug).OnDelete(DeleteBehavior.NoAction);;
        builder.HasMany(u => u.Likes).WithOne(t => t.User).HasForeignKey(t => t.UserSlug).OnDelete(DeleteBehavior.NoAction);;

    }
}