﻿namespace API.DTO
{
    public class MemberDto
    {
        public Guid UserId { get; set; }

        public string? UserName { get; set; }

        public int Age { get; set; }

        public string? KnownAs { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset LastActive { get; set; }

        public string? Gender { get; set; }

        public string? Introduction { get; set; }

        public string? Interests { get; set; }

        public string? LookingFor { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public string? PhotoUrl { get; set; }

        public List<PhotoDto>? Photos { get; set; }
    }
}
