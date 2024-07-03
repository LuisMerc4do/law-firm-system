
using law_firm_management.Data;
using law_firm_management.Dto.MessageDto;
using law_firm_management.interfaces;
using law_firm_management.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace law_firm_management.Repository
{
    public class MessageRepository : IMessageManager
    {
        private readonly ApplicationDBContext _context;
        private readonly IMemoryCache _cache;
        private readonly ILogger<MessageRepository> _logger;

        public MessageRepository(ApplicationDBContext context, IMemoryCache cache, ILogger<MessageRepository> logger)
        {
            _context = context;
            _cache = cache;
            _logger = logger;
        }

        public async Task<MessageModelDto> CreateMessageAsync(MessageModelDto messageDto)
        {
            var messageModel = new MessageModel
            {
                CaseId = messageDto.CaseId,
                Content = messageDto.Content,
                SenderId = messageDto.SenderId,
                DateSent = DateTime.Now // Example: Set the sent date
            };

            await _context.Messages.AddAsync(messageModel);
            await _context.SaveChangesAsync();

            _cache.Remove("messageRecords"); // Invalidate cache

            _logger.LogInformation("Message created and cache invalidated.");

            // Convert MessageModel to MessageModelDto and return
            return new MessageModelDto
            {
                MessageId = messageModel.MessageId,
                CaseId = messageModel.CaseId,
                Content = messageModel.Content,
                SenderId = messageModel.SenderId,
                DateSent = messageModel.DateSent
                // Include other properties as needed
            };
        }

        public async Task<MessageModelDto> DeleteMessageAsync(int id)
        {
            var messageModel = await _context.Messages.FirstOrDefaultAsync(x => x.MessageId == id);
            if (messageModel == null)
            {
                _logger.LogWarning("Message with ID {Id} not found.", id);
                return null;
            }

            _context.Messages.Remove(messageModel);
            await _context.SaveChangesAsync();

            _cache.Remove("messageRecords"); // Invalidate cache

            _logger.LogInformation("Message deleted and cache invalidated.");

            // Convert MessageModel to MessageModelDto and return
            return new MessageModelDto
            {
                MessageId = messageModel.MessageId,
                CaseId = messageModel.CaseId,
                Content = messageModel.Content,
                SenderId = messageModel.SenderId,
                DateSent = messageModel.DateSent
                // Include other properties as needed
            };
        }

        public async Task<List<MessageModelDto>> GetAllMessagesAsync()
        {
            var messageModels = await _context.Messages.ToListAsync();

            // Convert List<MessageModel> to List<MessageModelDto> and return
            return messageModels.Select(messageModel => new MessageModelDto
            {
                MessageId = messageModel.MessageId,
                CaseId = messageModel.CaseId,
                Content = messageModel.Content,
                SenderId = messageModel.SenderId,
                DateSent = messageModel.DateSent
                // Include other properties as needed
            }).ToList();
        }

        public async Task<MessageModelDto> GetMessageByIdAsync(int id)
        {
            var messageModel = await _context.Messages
                .Include(m => m.Case) // Include related Case if necessary
                .FirstOrDefaultAsync(x => x.MessageId == id);

            if (messageModel == null)
            {
                _logger.LogWarning("Message with ID {Id} not found.", id);
                return null;
            }

            // Convert MessageModel to MessageModelDto and return
            return new MessageModelDto
            {
                MessageId = messageModel.MessageId,
                CaseId = messageModel.CaseId,
                Content = messageModel.Content,
                SenderId = messageModel.SenderId,
                DateSent = messageModel.DateSent
                // Include other properties as needed
            };
        }

        public async Task<MessageModelDto> UpdateMessageAsync(int id, UpdateMessageDto messageDto)
        {
            var messageModel = await _context.Messages.FirstOrDefaultAsync(x => x.MessageId == id);
            if (messageModel == null)
            {
                _logger.LogWarning("Message with ID {Id} not found.", id);
                return null;
            }

            messageModel.Content = messageDto.Content;

            await _context.SaveChangesAsync();

            _cache.Remove("messageRecords"); // Invalidate cache

            _logger.LogInformation("Message updated and cache invalidated.");

            // Convert MessageModel to MessageModelDto and return
            return new MessageModelDto
            {
                MessageId = messageModel.MessageId,
                CaseId = messageModel.CaseId,
                Content = messageModel.Content,
                SenderId = messageModel.SenderId,
                DateSent = messageModel.DateSent
                // Include other properties as needed
            };
        }
    }
}
