using API.DTO;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public MessageController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var userName = User.GetUsername();

            if (userName == createMessageDto.RecipientUserName.ToLower())
            {
                return BadRequest("You cannot message yourself");
            }

            var sender = await _unitOfWork.UserRepository.GetUserByUsernameAsync(userName);
            var recipient = await _unitOfWork.UserRepository.GetUserByUsernameAsync(createMessageDto.RecipientUserName);

            if (sender is null || recipient is null)
            {
                return BadRequest("Cannot send message, sender or recipient was not found");
            }

            if (string.IsNullOrWhiteSpace(createMessageDto.Content))
            {
                return BadRequest("Message cannot be null or empty");
            }

            var messageEntity = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUserName = sender.UserName,
                RecipientUserName = recipient.UserName,
                Content = createMessageDto.Content
            };

            MessageDto message = await _unitOfWork.MessageRepository.AddMessageAsync(messageEntity);

            if (await _unitOfWork.CompleteAsync())
            {
                return Ok(message);
            }
            else
            {
                return BadRequest("Failed to save message");
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MessageDto>>> GetMessages([FromQuery] MessageParams messageParams)
        {
            messageParams.UserName = User.GetUsername();

            var messages = await _unitOfWork.MessageRepository.GetMessagesAsync(messageParams);

            Response.AddPaginationHeader(messages);

            return Ok(messages);
        }

        [HttpGet("thread/{userName}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string userName)
        {
            var currenUserName = User.GetUsername();

            var messages = await _unitOfWork.MessageRepository.GetMessageThread(currenUserName, userName);

            if (_unitOfWork.HasChanges())
            {
                await _unitOfWork.CompleteAsync();
            }

            return Ok(messages);
        }

        [HttpDelete("{messageId:Guid}")]
        public async Task<ActionResult> DeleteMessage(Guid messageId)
        {
            var userName = User.GetUsername();

            var message = await _unitOfWork.MessageRepository.GetMessageByIdAsync(messageId);

            if (message is null)
            {
                return BadRequest("Cannot delete the message");
            }

            if (message.SenderUserName != userName && message.RecipientUserName != userName)
            {
                return Forbid();
            }

            if (message.SenderUserName == userName) message.SenderDeleted = true;
            if (message.RecipientUserName == userName) message.RecipientDeleted = true;

            if (message is { SenderDeleted: true, RecipientDeleted: true })
            {
                _unitOfWork.MessageRepository.DeleteMessage(message);
            }

            if (await _unitOfWork.CompleteAsync()) return Ok();

            return BadRequest("Error deleting message");
        }
    }
}
