using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Entities
{
    public class Message
    {
        public Guid MessageId { get; set; }
        public required string SenderUserName { get; set; }
        public required string RecipientUserName { get; set; }
        public required string Content { get; set; }
        public DateTimeOffset? DateRead { get; set; }
        public DateTimeOffset MessageSent { get; set; } = DateTimeOffset.UtcNow;
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }

        // Navigation properties
        public Guid SenderId { get; set; }
        public User Sender { get; set; } = null!;
        public Guid RecipientId { get; set; }
        public User Recipient { get; set; } = null!;
    }

    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.Property(p => p.SenderUserName).HasMaxLength(128);
            builder.Property(p => p.RecipientUserName).HasMaxLength(128);
            builder.Property(p => p.Content).HasMaxLength(3000);

            builder.HasOne(x => x.Recipient)
                .WithMany(x => x.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Sender)
                .WithMany(x => x.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}