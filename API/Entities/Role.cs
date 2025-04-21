using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Entities
{
    public class Role : IdentityRole<Guid>
    {
        // Navigation properties
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }

    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");

            // Navigation properties.
            builder.HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(e => e.RoleId)
                .IsRequired();
        }
    }
}
