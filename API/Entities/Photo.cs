using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Entities
{
    public class Photo
    {
        public Guid PhotoId { get; set; }
        public required string Url { get; set; }
        public bool IsMain { get; set; }
        public string? PublicId { get; set; } // For cloudinary

        // Navigation properties
        ////public required User User { get; set; } // The required keyword breaks the seeding.
        public User User { get; set; } = null!;
        public Guid UserId { get; set; }
    }

    public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
    {
        public void Configure(EntityTypeBuilder<Photo> builder)
        {
            builder.HasKey(e => e.PhotoId);
            builder.Property(e => e.PhotoId).HasDefaultValueSql("NEWSEQUENTIALID()");
            builder.Property(e => e.Url).HasMaxLength(256);
            builder.Property(e => e.PublicId).HasMaxLength(256);

            // Navigation properties, not needed because it is alredy define by convention above.
            ////builder.HasOne(e => e.User)
            ////    .WithMany(e => e.Photos)
            ////    .HasForeignKey(e => e.UserId)
            ////    .IsRequired()
            ////    .OnDelete(DeleteBehavior.Cascade); // Optional: specify delete behavior
        }
    }
}