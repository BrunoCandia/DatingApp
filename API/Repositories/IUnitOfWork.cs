namespace API.Repositories
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IMessageRepository MessageRepository { get; }
        ILikeUserRepository LikeUserRepository { get; }
        Task<bool> CompleteAsync();
        bool HasChanges();
        void AttachEntity<T>(T entity) where T : class;
    }
}
