using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Entities
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public required User User { get; set; }
        public required Role Role { get; set; }
    }

    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            // Navigation properties, not needed because it is already defined by fluent in UserConfiguration/RoleConfiguration.
            ////builder.HasKey(e => new { e.UserId, e.RoleId });

            ////builder.HasOne(e => e.User)
            ////    .WithMany(e => e.UserRoles)
            ////    .HasForeignKey(e => e.UserId)
            ////    .IsRequired()
            ////    .OnDelete(DeleteBehavior.Cascade);

            ////builder.HasOne(e => e.Role)
            ////    .WithMany(e => e.UserRoles)
            ////    .HasForeignKey(e => e.RoleId)
            ////    .IsRequired()
            ////    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
