using API.Data;
using API.DTO;
using API.Entities;
using API.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace API.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _dataContext;

        public MessageRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<MessageDto> AddMessageAsync(Message message)
        {
            EntityEntry<Message> messageEntityEntry = await _dataContext.Messages.AddAsync(message);

            // Photos were eager loaded in GetUserByUsernameAsync method.

            MessageDto messageDto = new MessageDto
            {
                MessageId = messageEntityEntry.Entity.MessageId,
                SenderId = messageEntityEntry.Entity.SenderId,
                SenderUserName = messageEntityEntry.Entity.SenderUserName,
                SenderPhotoUrl = messageEntityEntry.Entity.Sender.Photos.FirstOrDefault(x => x.IsMain)!.Url,
                RecipientId = messageEntityEntry.Entity.RecipientId,
                RecipientUserName = messageEntityEntry.Entity.RecipientUserName,
                RecipientPhotoUrl = messageEntityEntry.Entity.Recipient.Photos.FirstOrDefault(x => x.IsMain)!.Url,
                Content = messageEntityEntry.Entity.Content,
                DateRead = messageEntityEntry.Entity.DateRead,
                MessageSent = messageEntityEntry.Entity.MessageSent
            };

            return messageDto;
        }

        public void DeleteMessage(Message message)
        {
            _dataContext.Messages.Remove(message);
        }

        public async Task<Message?> GetMessageByIdAsync(Guid messageId)
        {
            return await _dataContext.Messages.FindAsync(messageId);
        }

        public async Task<PagedList<MessageDto>> GetMessagesAsync(MessageParams messageParams)
        {
            var query = _dataContext.Messages
                .OrderByDescending(x => x.MessageSent)
                .AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(x => x.Recipient.UserName == messageParams.UserName && !x.RecipientDeleted),
                "Outbox" => query.Where(x => x.Sender.UserName == messageParams.UserName && !x.SenderDeleted),
                _ => query.Where(x => x.Recipient.UserName == messageParams.UserName && x.DateRead == null && !x.RecipientDeleted)
            };

            return await PagedList<MessageDto>.CreateAsync(
                query.Select(x => new MessageDto
                {
                    MessageId = x.MessageId,
                    SenderId = x.SenderId,
                    SenderUserName = x.SenderUserName,
                    SenderPhotoUrl = x.Sender.Photos.FirstOrDefault(x => x.IsMain)!.Url,
                    RecipientId = x.RecipientId,
                    RecipientUserName = x.RecipientUserName,
                    RecipientPhotoUrl = x.Recipient.Photos.FirstOrDefault(x => x.IsMain)!.Url,
                    Content = x.Content,
                    DateRead = x.DateRead,
                    MessageSent = x.MessageSent
                }),
                messageParams.PageNumber,
                messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string recipientUserName)
        {
            var messages = await _dataContext.Messages
                .Include(x => x.Sender).ThenInclude(x => x.Photos)
                .Include(x => x.Recipient).ThenInclude(x => x.Photos)
                .Where(x =>
                    x.RecipientUserName == currentUserName && !x.RecipientDeleted && x.SenderUserName == recipientUserName ||
                    x.SenderUserName == currentUserName && !x.SenderDeleted && x.RecipientUserName == recipientUserName)
                .OrderBy(x => x.MessageSent)
                .ToListAsync();

            var unReadMessages = messages.Where(x => x.DateRead == null && x.RecipientUserName == currentUserName).ToList();

            if (unReadMessages.Count != 0)
            {
                // Saving data in a get request is a bad practice!!!
                unReadMessages.ForEach(x => x.DateRead = DateTimeOffset.UtcNow);
                await _dataContext.SaveChangesAsync();
            }

            List<MessageDto> resultMessages = messages.Select(x => new MessageDto
            {
                MessageId = x.MessageId,
                SenderId = x.SenderId,
                SenderUserName = x.SenderUserName,
                SenderPhotoUrl = x.Sender.Photos.FirstOrDefault(x => x.IsMain)!.Url,
                RecipientId = x.RecipientId,
                RecipientUserName = x.RecipientUserName,
                RecipientPhotoUrl = x.Recipient.Photos.FirstOrDefault(x => x.IsMain)!.Url,
                Content = x.Content,
                DateRead = x.DateRead,
                MessageSent = x.MessageSent
            }).ToList();

            return resultMessages;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}
