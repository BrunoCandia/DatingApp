using API.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Entities
{
    public class User : IdentityUser<Guid>
    {
        ///public Guid UserId { get; set; }

        ////public required string UserName { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public required string KnownAs { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset LastActive { get; set; } = DateTimeOffset.UtcNow;

        public required string Gender { get; set; }

        public string? Introduction { get; set; }

        public string? Interests { get; set; }

        public string? LookingFor { get; set; }

        public required string City { get; set; }

        public required string Country { get; set; }

        public List<Photo> Photos { get; set; } = new List<Photo>();

        ////public List<UserLike> LikedByUsers { get; set; } = new List<UserLike>();

        ////public List<UserLike> LikedUsers { get; set; } = new List<UserLike>();

        ////public List<Message> MessagesSent { get; set; } = new List<Message>();

        ////public List<Message> MessagesReceived { get; set; } = new List<Message>();

        // Navigation properties
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        // TODO: How to get the age of the user and make it work wiht EF Core?
        public int GetAge()
        {
            return DateOfBirth.CalculateAge();
        }
    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");

            // Navigation properties.
            builder.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .IsRequired();
        }
    }
}
