
using API.Data;

namespace API.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;
        private readonly IUserRepository _userRepository;
        private readonly ILikeUserRepository _likeUserRepository;
        private readonly IMessageRepository _messageRepository;

        public UnitOfWork(DataContext dataContext, IUserRepository userRepository, ILikeUserRepository likeUserRepository, IMessageRepository messageRepository)
        {
            _dataContext = dataContext;
            _userRepository = userRepository;
            _likeUserRepository = likeUserRepository;
            _messageRepository = messageRepository;
        }

        public IUserRepository UserRepository => _userRepository;

        public IMessageRepository MessageRepository => _messageRepository;

        public ILikeUserRepository LikeUserRepository => _likeUserRepository;

        public void AttachEntity<T>(T entity) where T : class
        {
            _dataContext.Attach<T>(entity);
        }

        public async Task<bool> CompleteAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _dataContext.ChangeTracker.HasChanges();
        }
    }
}
