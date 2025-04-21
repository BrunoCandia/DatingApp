////using Microsoft.EntityFrameworkCore;
////using Microsoft.EntityFrameworkCore.Metadata.Builders;

////namespace API.Entities
////{
////    public class UserLike
////    {
////        public required User SourceUser { get; set; }
////        public Guid SourceUserId { get; set; }
////        public required User TargetUser { get; set; }
////        public Guid TargetUserId { get; set; }
////    }

////    public class UserLikeConfiguration : IEntityTypeConfiguration<UserLike>
////    {
////        public void Configure(EntityTypeBuilder<UserLike> builder)
////        {
////            builder.HasKey(e => new { e.SourceUserId, e.TargetUserId });
////        }
////    }
////}