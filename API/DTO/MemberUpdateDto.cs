namespace API.DTO
{
    public class MemberUpdateDto
    {
        public string? Introdcution { get; set; }
        public string? LookingFor { get; set; }
        public string? Interests { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
    }
}
