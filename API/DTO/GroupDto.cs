namespace API.DTO
{
    public class GroupDto
    {
        public required string Name { get; set; }
        public List<ConnectionDto> Connections { get; set; } = new List<ConnectionDto>();
    }
}
