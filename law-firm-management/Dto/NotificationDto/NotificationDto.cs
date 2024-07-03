using law_firm_management.Dto.AccountDto;

namespace law_firm_management.Dto.NotificationDto;

public class NotificationModelDto
{
    public int NotificationId { get; set; }
    public string UserId { get; set; }
    public UserDto User { get; set; }
    public string Message { get; set; }
    public DateTime DateSent { get; set; }
}
