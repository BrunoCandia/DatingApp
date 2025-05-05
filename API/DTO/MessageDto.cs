namespace API.DTO
{
    public class MessageDto
    {
        public Guid MessageId { get; set; }
        public Guid SenderId { get; set; }
        public required string SenderUserName { get; set; }
        public required string SenderPhotoUrl { get; set; }
        public Guid RecipientId { get; set; }
        public required string RecipientUserName { get; set; }
        public required string RecipientPhotoUrl { get; set; }
        public required string Content { get; set; }
        public DateTimeOffset? DateRead { get; set; }
        public DateTimeOffset MessageSent { get; set; }
    }
}
