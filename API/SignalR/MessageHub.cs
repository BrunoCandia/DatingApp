using API.DTO;
using API.Entities;
using API.Extensions;
using API.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class MessageHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<PresenceHub> _presenceHubContext;

        public MessageHub(IUnitOfWork unitOfWork, IHubContext<PresenceHub> presenceHubContext)
        {
            _unitOfWork = unitOfWork;
            _presenceHubContext = presenceHubContext;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext?.Request.Query["user"];

            if (Context.User is null || string.IsNullOrWhiteSpace(otherUser))
            {
                throw new Exception("Cannot join group");
            }

            var groupName = GetGroupName(Context.User.GetUsername(), otherUser!);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var groupDto = await AddToGroupAsync(groupName);

            await Clients.Group(groupName).SendAsync("UpdatedGroup", groupDto);

            var messages = await _unitOfWork.MessageRepository.GetMessageThread(Context.User.GetUsername(), otherUser!);

            if (_unitOfWork.HasChanges())
            {
                await _unitOfWork.CompleteAsync();
            }

            await Clients.Caller.SendAsync("ReceiveMessageThread", messages);

            ////await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var groupDto = await RemoveConnectionFromGroupAsync();

            await Clients.Group(groupDto.Name).SendAsync("UpdatedGroup", groupDto);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var userName = Context.User?.GetUsername() ?? throw new Exception("Cannot get user");

            if (userName == createMessageDto.RecipientUserName.ToLower())
            {
                throw new HubException("You cannot message yourself");
            }

            var sender = await _unitOfWork.UserRepository.GetUserByUsernameAsync(userName);
            var recipient = await _unitOfWork.UserRepository.GetUserByUsernameAsync(createMessageDto.RecipientUserName);

            if (sender is null || recipient is null)
            {
                throw new HubException("Cannot send message, sender or recipient was not found");
            }

            if (string.IsNullOrWhiteSpace(createMessageDto.Content))
            {
                throw new HubException("Message cannot be null or empty");
            }

            var messageEntity = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUserName = sender.UserName,
                RecipientUserName = recipient.UserName,
                Content = createMessageDto.Content
            };

            var groupName = GetGroupName(sender.UserName, recipient.UserName);
            var groupDto = await _unitOfWork.MessageRepository.GetMessageGroupAsync(groupName);

            if (groupDto is not null && groupDto.Connections.Any(x => x.UserName == recipient.UserName))
            {
                messageEntity.DateRead = DateTimeOffset.UtcNow;
            }
            else
            {
                var connections = await PresenceTracker.GetConnectionsForUser(recipient.UserName);

                if (connections is not null && connections.Count != 0)
                {
                    await _presenceHubContext.Clients.Clients(connections).SendAsync("NewMessageReceived", new { userName = sender.UserName, knownAs = sender.KnownAs });
                }
            }

            MessageDto message = await _unitOfWork.MessageRepository.AddMessageAsync(messageEntity);

            if (await _unitOfWork.CompleteAsync())
            {
                ////var groupName = GetGroupName(sender.UserName, recipient.UserName);

                await Clients.Group(groupName).SendAsync("NewMessage", message);
            }
        }

        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;

            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }

        private async Task<GroupDto> AddToGroupAsync(string groupName)
        {
            // Get the username of the current user
            var userName = Context.User?.GetUsername() ?? throw new HubException("Cannot get user name");

            // Check if the group already exists
            var groupDto = await _unitOfWork.MessageRepository.GetMessageGroupAsync(groupName);

            // Create a new connection for the current user
            var connection = new Connection
            {
                ConnectionId = Context.ConnectionId,
                UserName = userName
            };

            Group group;

            if (groupDto is null)
            {
                // If the group does not exist, create a new group
                group = new Group { Name = groupName };

                // Add the group to the database
                await _unitOfWork.MessageRepository.AddGroupAsync(group);
            }
            else
            {
                // If the group exists, retrieve it
                group = new Group
                {
                    Name = groupDto.Name,
                    Connections = groupDto.Connections.Select(c => new Connection
                    {
                        ConnectionId = c.ConnectionId,
                        UserName = c.UserName
                    }).ToList()
                };

                // Attach the group to the tracker
                _unitOfWork.AttachEntity(group);
            }

            // Add the connection to the group
            group.Connections.Add(connection);

            // Save changes to the database
            if (await _unitOfWork.CompleteAsync())
            {
                return new GroupDto
                {
                    Name = group.Name,
                    Connections = group.Connections.Select(x => new ConnectionDto
                    {
                        ConnectionId = x.ConnectionId,
                        UserName = x.UserName,
                    }).ToList()
                };
            }

            throw new HubException("Failed to join group");
        }

        private async Task<GroupDto> RemoveConnectionFromGroupAsync()
        {
            var groupDto = await _unitOfWork.MessageRepository.GetGroupForConnectionAsync(Context.ConnectionId);
            var connectionDto = groupDto?.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            //var connectionDto = await _messageRepository.GetConnectionAsync(Context.ConnectionId);

            if (groupDto is not null && connectionDto is not null)
            {
                var connection = new Connection { ConnectionId = connectionDto.ConnectionId, UserName = connectionDto.UserName };
                _unitOfWork.MessageRepository.RemoveConnection(connection);

                if (await _unitOfWork.CompleteAsync())
                {
                    return groupDto;
                }
            }

            throw new HubException("Failed to remove connection from group");
        }
    }
}
