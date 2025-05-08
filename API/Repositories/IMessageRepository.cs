using API.DTO;
using API.Entities;
using API.Helpers;

namespace API.Repositories
{
    public interface IMessageRepository
    {
        void AttachGroup(Group group);
        Task<MessageDto> AddMessageAsync(Message message);
        void DeleteMessage(Message message);
        Task<Message?> GetMessageByIdAsync(Guid messageId);
        Task<PagedList<MessageDto>> GetMessagesAsync(MessageParams messageParams);
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string recipientUserName);

        Task AddGroupAsync(Group group);
        void RemoveConnection(Connection connection);
        Task<ConnectionDto?> GetConnectionAsync(string connectionId);
        Task<GroupDto?> GetMessageGroupAsync(string groupName);
        Task<GroupDto?> GetGroupForConnectionAsync(string connecationId);
    }
}
