using law_firm_management.Dto.MessageDto;

namespace law_firm_management.interfaces;

public interface IMessageManager
{
    Task<MessageModelDto> CreateMessageAsync(MessageModelDto messageDto);
    Task<MessageModelDto> GetMessageByIdAsync(int messageId);
    Task<List<MessageModelDto>> GetAllMessagesAsync();
    Task<MessageModelDto> UpdateMessageAsync(int messageId, UpdateMessageDto messageDto);
    Task<MessageModelDto> DeleteMessageAsync(int messageId);
}
