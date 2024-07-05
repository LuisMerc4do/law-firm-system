using law_firm_management.Dto.MessageDto;
using law_firm_management.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace law_firm_management.Controllers
{
    [Route("api/v1/messages")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageManager _messageManager;
        private readonly ILogger<MessageController> _logger;

        public MessageController(IMessageManager messageManager, ILogger<MessageController> logger)
        {
            _messageManager = messageManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMessages()
        {
            try
            {
                var messages = await _messageManager.GetAllMessagesAsync();
                return Ok(messages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all messages.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessageById(int id)
        {
            try
            {
                var messageDto = await _messageManager.GetMessageByIdAsync(id);
                if (messageDto == null)
                {
                    return NotFound();
                }
                return Ok(messageDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the message with ID {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] CreateMessageDto createMessageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdMessage = await _messageManager.CreateMessageAsync(new MessageModelDto
                {
                    CaseId = createMessageDto.CaseId,
                    SenderId = createMessageDto.SenderId,
                    Content = createMessageDto.Content,
                    DateSent = DateTime.Now // Set sent date here
                });

                return CreatedAtAction(nameof(GetMessageById), new { id = createdMessage.MessageId }, createdMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new message.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMessage(int id, [FromBody] UpdateMessageDto updateMessageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedMessage = await _messageManager.UpdateMessageAsync(id, updateMessageDto);
                if (updatedMessage == null)
                {
                    return NotFound();
                }

                return Ok(updatedMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the message with ID {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            try
            {
                var deletedMessage = await _messageManager.DeleteMessageAsync(id);
                if (deletedMessage == null)
                {
                    return NotFound();
                }

                return Ok(deletedMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the message with ID {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
