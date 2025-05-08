using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Entities
{
    public class Group
    {
        public required string Name { get; set; }

        //The Connections property suggests a one-to-many relationship between Group and Connection.
        //However, the relationship is not explicitly configured in this file, so EF Core will infer it based on conventions.
        public ICollection<Connection> Connections { get; set; } = new List<Connection>();
    }

    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(p => p.Name);
            builder.Property(p => p.Name).HasMaxLength(50);

            // Explicitly define the One-to-Many relationship configuration
            builder.HasMany(g => g.Connections)           // A Group has many Connections
                   .WithOne()                                              // Each Connection belongs to one Group
                   .HasForeignKey("GroupName")          // Foreign key in Connection table
                   .OnDelete(DeleteBehavior.Cascade);          // Cascade delete when a Group is deleted
        }
    }
}
