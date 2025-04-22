using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class RegisterDto
    {
        public required string UserName { get; set; }

        [StringLength(8, MinimumLength = 4)]
        public required string Password { get; set; }

        public required string DateOfBirth { get; set; }

        public required string KnownAs { get; set; }

        public required string Gender { get; set; }

        public required string City { get; set; }

        public required string Country { get; set; }
    }
}
