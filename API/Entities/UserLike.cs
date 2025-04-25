using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Entities
{
    public class UserLike
    {
        public required User SourceUser { get; set; }
        public Guid SourceUserId { get; set; }
        public required User TargetUser { get; set; }
        public Guid TargetUserId { get; set; }
    }

    public class UserLikeConfiguration : IEntityTypeConfiguration<UserLike>
    {
        public void Configure(EntityTypeBuilder<UserLike> builder)
        {
            builder.HasKey(e => new { e.SourceUserId, e.TargetUserId });

            // Configure the many-to-many relationship for LikedByUsers and LikedUsers.

            // 1.   LikedByUsers Configuration:
            //  Configure the relationship for TargetUser
            //  Sets the foreign key as TargetUserId.
            builder.HasOne(ul => ul.TargetUser)
                .WithMany(u => u.LikedByUsers)
                .HasForeignKey(ul => ul.TargetUserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 2.   LikedUsers Configuration:
            //  Configure the relationship for SourceUser.
            //  Sets the foreign key as SourceUserId.
            builder.HasOne(ul => ul.SourceUser)
                .WithMany(u => u.LikedUsers)
                .HasForeignKey(ul => ul.SourceUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}