using law_firm_management.Dto.AccountDto;
using law_firm_management.Dto.CaseDto;

namespace law_firm_management.Dto.MessageDto;

public class MessageModelDto
{
    public int MessageId { get; set; }
    public int CaseId { get; set; }
    public CaseModelDto Case { get; set; }
    public string SenderId { get; set; }
    public UserDto Sender { get; set; }
    public string Content { get; set; }
    public DateTime DateSent { get; set; }
}
