using API.DTO;
using API.Entities;
using API.Helpers;

namespace API.Repositories
{
    public interface IMessageRepository
    {
        Task<MessageDto> AddMessageAsync(Message message);
        void DeleteMessage(Message message);
        Task<Message?> GetMessageByIdAsync(Guid messageId);
        Task<PagedList<MessageDto>> GetMessagesAsync(MessageParams messageParams);
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string recipientUserName);
        Task<bool> SaveAllAsync();
    }
}
