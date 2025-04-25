namespace API.Helpers
{
    public class LikeParams : PaginationParams
    {
        public Guid UserId { get; set; }

        public required string Predicate { get; set; } = "liked";
    }
}
