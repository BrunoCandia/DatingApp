using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Entitites
{
    public class User
    {
        public Guid UserId { get; set; }
        public required string UserName { get; set; }
    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.UserId);
            builder.Property(e => e.UserId).HasDefaultValueSql("NEWSEQUENTIALID()");
            builder.Property(e => e.UserName).HasMaxLength(50);
        }
    }
}
