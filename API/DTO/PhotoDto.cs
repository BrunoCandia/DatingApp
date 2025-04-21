namespace API.DTO
{
    public class PhotoDto
    {
        public Guid PhotoId { get; set; }
        public string? Url { get; set; }
        public bool IsMain { get; set; }
    }
}
