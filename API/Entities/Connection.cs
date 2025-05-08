using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Entities
{
    public class Connection
    {
        public required string ConnectionId { get; set; }
        public required string UserName { get; set; }

    }

    public class ConnectionConfiguration : IEntityTypeConfiguration<Connection>
    {
        public void Configure(EntityTypeBuilder<Connection> builder)
        {
            builder.HasKey(p => p.ConnectionId);
            builder.Property(p => p.ConnectionId).HasMaxLength(100);
        }
    }
}
